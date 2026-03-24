using UnityEngine;
using TMPro;

public class KToEnd : MonoBehaviour
{
    public Driftmanager driftManager;
    public GameObject endUI;       // De canvas die verschijnt als target bereikt is
    public GameObject kHintUI;     // De “Press K” hint die we willen verbergen
    public int targetScore = 1100; // Score wanneer de UI verschijnt
    private bool hasShown = false; // voorkomt dat de UI meerdere keren verschijnt

    void Start()
    {
        if(endUI != null) endUI.SetActive(false);
        if(kHintUI != null) kHintUI.SetActive(false); // begin zonder hint
    }

    void Update()
    {
        float driftScore = GetTotalScore();
        Debug.Log("Current Drift Score: " + driftScore); // check of score echt stijgt

        // Check of target bereikt is en de hint nog niet is getoond
        if (!hasShown && driftScore >= targetScore)
        {
            if(kHintUI != null) kHintUI.SetActive(true); // toon de hint
            hasShown = true;
            Debug.Log("Score target bereikt! Score: " + driftScore);
        }

        // Check of speler K drukt om het spel te eindigen
        if (hasShown && Input.GetKeyDown(KeyCode.K))
        {
            ShowEndCanvas();
        }
    }

    void ShowEndCanvas()
    {
        Debug.Log("Game Ended!");

        // Canvas zichtbaar maken
        if(endUI != null) endUI.SetActive(true);

        // Hint wegdoen
        if(kHintUI != null) kHintUI.SetActive(false);

        // Pauzeer de game (optioneel)
        Time.timeScale = 0f;

        // Scene load kan hier eventueel:
        // SceneManager.LoadScene("MainMenu");
    }

    private float GetTotalScore()
    {
        // Haalt de score uit de DriftManager
        return driftManager.totalScore;
    }
}