using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class Driftmanager : MonoBehaviour
{

    public Rigidbody playerRB;
    public TMP_Text totalScoreText; 
    public TMP_Text currentScoreText;
    public TMP_Text MultiplierText;
    public TMP_Text driftAngleText;

    private float speed=0;
    private float driftAngle=0;
    private float driftMultiplier=1;
    private float currentScore;
    public float totalScore = 1000f;

    private bool isDrifting = false;

    public float minimunSpeed = 5;
    public float minimumAngle = 10;
    public float driftingDelay = 0.2f;
    public GameObject driftingObject;
    public Color normalDriftColor;
    public Color nearStopDriftColor;
    public Color failDriftColor;

    private XPReceiver xpReceiver;
    
    
    private IEnumerator stopDriftingCoroutine = null;
    void Start()
    {
        driftingObject.SetActive(false);
        xpReceiver = GetComponent<XPReceiver>();
    }

 
    void Update()
    {
        ManageDrift();
        ManageUI();
    }
    void ManageDrift()
    {
        //speed is dus hoe snel hij gaat
        speed = playerRB.linearVelocity.magnitude;
        //dit is de angle van de auto, die wordt uitgerekent door de forward van de auto en de kant waarop de auto slide en dat wordt meegegeven aan de driftingAngle.
        //en als de auto stil staat is de forward de voorkant van de auto en dat is genormalizeerd.
        driftAngle = Vector3.Angle(playerRB.transform.forward, (playerRB.linearVelocity + playerRB.transform.forward).normalized);
        //Als de forward van de auto te ver van de velocity is gedraaid (verder dan 120 graden) dan is hij gefaald en gaat naar 0. 
        if (driftAngle>120)
        {
            driftAngle = 0;
        }
        //Als de drift angle groter is dan de minimumangle en speed hoger is dan minimum speed dan return true en startDrifting als niet dan stopDrift
        if (driftAngle > minimumAngle && speed > minimunSpeed)
        {
            if (!isDrifting)
            {
                StartDrift();
            }

            if (isDrifting)
            {
                currentScore += Time.deltaTime * driftAngle * driftMultiplier;
                driftMultiplier += Time.deltaTime;
                driftingObject.SetActive(true);
            }
        }
        else
        {
            if (isDrifting && stopDriftingCoroutine == null)
            {
                StopDrift();
            }
        }
    }
    //We maken een async aan dit kan zorgen dat het later in beeld komt zodat het smooth voelt, dit doen we dus door als niet aan het driften zijn om dan het systeem
    // een delay te geven wat in miliseconden is dus dat is de drifting delay * 1000 zodat het in seconden is
    async void StartDrift()
    {
        if (!isDrifting)
        {
            await Task.Delay(Mathf.RoundToInt(1000*driftingDelay));
            driftMultiplier = 1; 
        }
        if(stopDriftingCoroutine!=null)
        {
            StopCoroutine(stopDriftingCoroutine);
            stopDriftingCoroutine = null;
        }
        currentScoreText.color = normalDriftColor;
        isDrifting = true;
    }
    //Dit wat zorgt dat de driftscore gaat stoppen, we gebruiken een Coroutine zodat taken gepauzeerd en hervat kunnen worden
    //Wat de eerste lijn aan code doet is dat hij StoppingDrift op Null zet maar dat wordt gedaan door eerst de IEnumerator te activeren
    //Dan begint de StoppingDrift te vuren en dan in de eerste wacht hij .1 seconde en laat de nearStopColor zien
    //Daarna wacht hij 4 secondens en voegt de currentscore op de totalscore en als niet aan het driften is laat hij de FailedDriftcolor zien
    //Daarna wacht hij 0.5 secondes en zet de currentscore op 0 en haald de UI weg van het driften
    void StopDrift()
    {
        stopDriftingCoroutine= StoppingDrift();
        StartCoroutine(stopDriftingCoroutine);
    }
    private IEnumerator StoppingDrift()
    {
        yield return new WaitForSeconds(0.1f);
        currentScoreText.color=nearStopDriftColor;
        yield return new WaitForSeconds(driftingDelay * 4f);
        totalScore += currentScore;
        if (xpReceiver != null)
        xpReceiver.GiveXPFromDrift((int)currentScore);
        isDrifting = false;
        currentScoreText.color=failDriftColor;
        yield return new WaitForSeconds(0.5f);
        currentScore=0;
        driftingObject.SetActive(false);
    }
    void ManageUI()
    {
        //Dit zorgt ervoor dat totalScore text veranderd naar de totalscore en dat een string maakt en ervoor zorgt dat de score in miljoenen en duizende een komma heeft, 
        // maar niet wanneer het een getal onder de duizend is
        //De multiplierText veranderd naar een string wat standaard 0 is maar als het veranderd een komma getal kan worden
        totalScoreText.text="Total: "+totalScore.ToString(".##");
        MultiplierText.text=driftMultiplier.ToString("0,0")+"X";
        currentScoreText.text = currentScore.ToString("0.##");
        driftAngleText.text=driftAngle.ToString("000")+"°";
    }

}
