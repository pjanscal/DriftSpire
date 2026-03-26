using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks; // Nodig voor async/await (wachten zonder coroutine)

public class Driftmanager : MonoBehaviour
{

    public Rigidbody playerRB; // De Rigidbody van de auto → hiermee krijgen we velocity (snelheid + richting)
    public TMP_Text totalScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text MultiplierText;
    public TMP_Text driftAngleText;

    private float speed = 0; // huidige snelheid van de auto
    private float driftAngle = 0; // hoek tussen waar auto kijkt en waar hij heen beweegt
    private float driftMultiplier = 1; // multiplier die groter wordt hoe langer je drift
    private float currentScore; // score van huidige drift (nog niet opgeslagen)
    public float totalScore = 0f; // totale score over alle drifts

    private bool isDrifting = false; // checkt of je momenteel aan het driften bent

    public float minimunSpeed = 5; // minimale snelheid om te kunnen driften
    public float minimumAngle = 10; // minimale hoek om als drift te tellen
    public float driftingDelay = 0.2f; // kleine delay voordat drift echt start
    public GameObject driftingObject; // UI dat laat zien dat je drift
    public Color normalDriftColor;
    public Color nearStopDriftColor;
    public Color failDriftColor;

    private XPReceiver xpReceiver; // script dat XP ontvangt (koppeling naar XP systeem)

    //  QUEST MANAGER REFERENTIE
    public QuestManager questManager;

    //  NIEUW: hoogste multiplier tijdens deze drift
    private float maxMultiplierReached = 0;

    private IEnumerator stopDriftingCoroutine = null; // referentie naar coroutine → zodat we hem kunnen stoppen

    void Start()
    {
        driftingObject.SetActive(false); // zet drift UI uit bij start
        xpReceiver = GetComponent<XPReceiver>(); // zoekt XPReceiver op dit object (zelfde auto)
    }

    void Update()
    {
        ManageDrift(); // alle drift logica
        ManageUI();    // UI updaten
    }

    void ManageDrift()
    {
        // speed is dus hoe snel hij gaat
        speed = playerRB.linearVelocity.magnitude;
        // magnitude = lengte van vector → dus snelheid zonder richting

        // driftAngle = hoek tussen waar auto kijkt en waar hij heen glijdt
        driftAngle = Vector3.Angle(
            playerRB.transform.forward,
            (playerRB.linearVelocity + playerRB.transform.forward).normalized
        );
        // Vector3.Angle = berekent hoek tussen 2 richtingen (in graden)
        // normalized = maakt vector lengte 1 (alleen richting telt)

        // Als de auto te ver draait (spin), dan telt het niet meer als drift
        if (driftAngle > 120)
        {
            driftAngle = 0;
        }

        // Als angle en snelheid hoog genoeg zijn → we zijn aan het driften
        if (driftAngle > minimumAngle && speed > minimunSpeed)
        {
            if (!isDrifting)
            {
                StartDrift(); // start drift als we dat nog niet waren
                maxMultiplierReached = 0; //  reset bij nieuwe drift
            }

            if (isDrifting)
            {
                // Score groeit over tijd
                float scoreGain = Time.deltaTime * driftAngle * driftMultiplier;
                currentScore += scoreGain;

                driftMultiplier += Time.deltaTime; // multiplier groeit hoe langer je drift

                driftingObject.SetActive(true); // toon drift UI/effect

                //  HOOGSTE MULTIPLIER BIJHOUDEN
                if (driftMultiplier > maxMultiplierReached)
                {
                    maxMultiplierReached = driftMultiplier;
                }

                //  QUEST PROGRESS (CONTINUOUS)
                if (questManager != null)
                {
                    questManager.AddProgress(QuestType.DriftTime, Time.deltaTime);
                    questManager.AddProgress(QuestType.DriftScore, scoreGain);

                    //  QUEST CHECKS (REAL-TIME)
                    questManager.CheckQuest(QuestType.DriftAngle, driftAngle);
                    questManager.CheckQuest(QuestType.MaxMultiplier, driftMultiplier);
                }
            }
        }
        else
        {
            // Als je stopt met driften
            if (isDrifting && stopDriftingCoroutine == null)
            {
                StopDrift(); // start stop-proces (met delay)
            }
        }
    }

    // async = alternatief voor coroutine → kan wachten zonder Unity coroutine systeem
    // await Task.Delay = wacht X milliseconden (niet frames!)
    async void StartDrift()
    {
        if (!isDrifting)
        {
            // wacht een klein beetje voordat drift echt begint
            await Task.Delay(Mathf.RoundToInt(1000 * driftingDelay));
            driftMultiplier = 1; // reset multiplier
        }

        // Als we nog een stop coroutine hadden → stop die
        if (stopDriftingCoroutine != null)
        {
            StopCoroutine(stopDriftingCoroutine);
            stopDriftingCoroutine = null;
        }

        currentScoreText.color = normalDriftColor; // zet kleur naar normaal
        isDrifting = true; // we zijn nu officieel aan het driften
    }

    // Start de coroutine die drift netjes stopt
    void StopDrift()
    {
        stopDriftingCoroutine = StoppingDrift(); // maak coroutine
        StartCoroutine(stopDriftingCoroutine);  // start coroutine
    }

    private IEnumerator StoppingDrift()
    {
        // wacht 0.1 seconde → kleine buffer zodat drift niet direct stopt
        yield return new WaitForSeconds(0.1f);

        currentScoreText.color = nearStopDriftColor; // kleur verandert (bijna stoppen)

        // wacht nog een beetje (afhankelijk van driftingDelay)
        yield return new WaitForSeconds(driftingDelay * 4f);

        totalScore += currentScore; // voeg current drift toe aan totaal

        // geef XP op basis van drift score
        if (xpReceiver != null)
            xpReceiver.GiveXPFromDrift((int)currentScore);

        // quest checks (einde van drift)
        if (questManager != null)
        {
            questManager.CheckQuest(QuestType.SingleDriftScore, currentScore);
            questManager.CheckQuest(QuestType.SingleDriftTime, maxMultiplierReached);
        }

        isDrifting = false; // drift is klaar

        currentScoreText.color = failDriftColor; // kleur verandert naar fail

        yield return new WaitForSeconds(0.5f); // kleine pauze voor reset

        currentScore = 0; // reset huidige score
        driftingObject.SetActive(false); // verberg drift UI
    }

    void ManageUI()
    {
        // ToString(".##") = max 2 decimalen tonen
        totalScoreText.text = "Total: " + totalScore.ToString(".##");

        // "0,0" = duizendtallen met komma (bijv 1,000)
        MultiplierText.text = driftMultiplier.ToString("0,0") + "X";

        currentScoreText.text = currentScore.ToString("0.##");

        // "000" = altijd 3 cijfers (bijv 005°)
        driftAngleText.text = driftAngle.ToString("000") + "°";
    }
}