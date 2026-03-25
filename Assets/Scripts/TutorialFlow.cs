using UnityEngine;
using System.Collections;

public class TutorialFlow : MonoBehaviour
{
    // CanvasGroups voor de verschillende tutorial hints (UI elementen)
    public CanvasGroup gasHint;
    public CanvasGroup steerHint;
    public CanvasGroup driftHint;

    // Houdt bij in welke stap van de tutorial we zitten (0 = gas, 1 = sturen, 2 = drift)
    private int currentStep = 0;

    // Voorkomt dat meerdere fades tegelijk gebeuren.
    private bool isTransitioning = false;

    void Start()
    {
        // Bij het starten:
        // Zet alle hints uit behalve de eerste (gasHint)

        HideInstant(steerHint); // stuur hint meteen uitzetten
        HideInstant(driftHint); // drift hint meteen uitzetten
        ShowInstant(gasHint);   // gas hint meteen laten zien

        currentStep = 0; // begin bij stap 0 (gas)
    }

    void Update()
    {
        // Als we aan het transitioningen zijn dan stop de rest van deze update, als hij weer false is, dan weer door gaan met de update.
        // dus als true = return, als false = skip deze regel code
        if (isTransitioning) return;

        
        // STEP 0 → Gas (W indrukken)
      
        if (currentStep == 0 && Input.GetKeyDown(KeyCode.W))
        {
            // Start een coroutine die gasHint uitfade en steerHint infade
            StartCoroutine(NextStep(gasHint, steerHint));

            currentStep++; // ga naar stap 1
        }

        
        // STEP 1 → Sturen (A of D)
       
        else if (currentStep == 1 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            // Fade van steerHint → driftHint
            StartCoroutine(NextStep(steerHint, driftHint));

            currentStep++; // ga naar stap 2
        }

       
        // STEP 2 → Drift (Spatie)
  
        else if (currentStep == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            // Fade de laatste hint uit (geen volgende stap)
            StartCoroutine(FadeOut(driftHint));

            currentStep++; // tutorial klaar
        }
    }

    // Deze coroutine regelt de overgang van één hint naar de volgende
    //IEnumerator is een lijstje wat hij afwerkt en yield return zorgt dat hij even wacht en daarna doorgaat, inplaats van alles tegerlijkertijd.

    IEnumerator NextStep(CanvasGroup current, CanvasGroup next)
    {
        isTransitioning = true; // blokkeer andere input tijdens overgang

        // Fade eerst de huidige hint uit
        yield return FadeOut(current);

        // Fade daarna de volgende hint in
        yield return FadeIn(next);

        isTransitioning = false; // overgang klaar, input weer toegestaan
    }

    // Fade IN functie (van onzichtbaar → zichtbaar)
    // Hij verwacht een CanvasGroup in deze IEnumerator en die wordt dan cg genoemt
    
    IEnumerator FadeIn(CanvasGroup cg)
    {
        cg.gameObject.SetActive(true); // zorg dat object actief is
        float t = 0; // start transparantie waarde

        // Loop totdat volledig zichtbaar
        while (t < 1)
        {
            t += Time.deltaTime * 2f; // verhoog t elke frame (2f = snelheid)
            cg.alpha = t;             // stel alpha in (opacity)
            yield return null;        // wacht 1 frame
        }

        cg.alpha = 1; // zorg dat hij exact 100% zichtbaar is
    }

    // Fade OUT functie (van zichtbaar → onzichtbaar)
    IEnumerator FadeOut(CanvasGroup cg)
    {
        float t = 1; // begin volledig zichtbaar

        // Loop totdat volledig onzichtbaar
        while (t > 0)
        {
            t -= Time.deltaTime * 2f; // verlaag t elke frame
            cg.alpha = t;             // pas transparantie aan
            yield return null;        // wacht 1 frame
        }

        cg.alpha = 0; // volledig onzichtbaar
        cg.gameObject.SetActive(false); // zet object uit (bespaart performance)
    }

    // Laat een hint meteen zien (zonder animatie)
    void ShowInstant(CanvasGroup cg)
    {
        cg.alpha = 1;                // volledig zichtbaar
        cg.gameObject.SetActive(true); // object aanzetten
    }

    // Verberg een hint meteen (zonder animatie)
    void HideInstant(CanvasGroup cg)
    {
        cg.alpha = 0;                 // volledig onzichtbaar
        cg.gameObject.SetActive(false); // object uitzetten
    }
}