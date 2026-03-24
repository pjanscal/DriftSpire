using UnityEngine;
using System.Collections;

public class TutorialFlow : MonoBehaviour
{
    public CanvasGroup gasHint;
    public CanvasGroup steerHint;
    public CanvasGroup driftHint;

    private int currentStep = 0;
    private bool isTransitioning = false;

    void Start()
    {
        // Alles uit behalve eerste hint
        HideInstant(steerHint);
        HideInstant(driftHint);
        ShowInstant(gasHint);

        currentStep = 0;
    }

    void Update()
    {
        if (isTransitioning) return;

        // STEP 0 → Gas
        if (currentStep == 0 && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(NextStep(gasHint, steerHint));
            currentStep++;
        }

        // STEP 1 → Sturen
        else if (currentStep == 1 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            StartCoroutine(NextStep(steerHint, driftHint));
            currentStep++;
        }

        // STEP 2 → Drift
        else if (currentStep == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeOut(driftHint));
            currentStep++;
        }
    }

    IEnumerator NextStep(CanvasGroup current, CanvasGroup next)
    {
        isTransitioning = true;

        yield return FadeOut(current);
        yield return FadeIn(next);

        isTransitioning = false;
    }

    IEnumerator FadeIn(CanvasGroup cg)
    {
        cg.gameObject.SetActive(true);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2f; // speed
            cg.alpha = t;
            yield return null;
        }

        cg.alpha = 1;
    }

    IEnumerator FadeOut(CanvasGroup cg)
    {
        float t = 1;

        while (t > 0)
        {
            t -= Time.deltaTime * 2f;
            cg.alpha = t;
            yield return null;
        }

        cg.alpha = 0;
        cg.gameObject.SetActive(false);
    }

    void ShowInstant(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.gameObject.SetActive(true);
    }

    void HideInstant(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.gameObject.SetActive(false);
    }
}