using UnityEngine;
using TMPro;

public class TotalScoreUI : MonoBehaviour
{
    public Driftmanager driftManager;
    private float totalScoreValue; // nieuwe naam
    public TMP_Text totalScoreText; // TMP_Text component

    void Start()
    {
        // optioneel: check of driftManager is ingesteld
        if (driftManager == null)
            driftManager = FindObjectOfType<Driftmanager>();
    }

    void Update()
    {
        totalScoreValue = GetTotalScore();
        totalScoreText.text = "Total: " + totalScoreValue.ToString(""); 
    }

    private float GetTotalScore()
    {
        // Haalt de score uit de DriftManager
        if (driftManager != null)
            return driftManager.totalScore;
        else
            return 0f;
    }
}