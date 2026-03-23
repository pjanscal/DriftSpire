using UnityEngine;
using UnityEngine.InputSystem;

public class MissionReward : MonoBehaviour
{
    public Driftmanager driftManager; // Haalt de drift score op
    public CurrencyManager currencyManager; // Geeft coins aan de speler

    public float multiplier = 1.5f; // Hoeveel de score waard is (bijv. 1.5x)

    void Update()
    {
        // Checkt of de K toets wordt ingedrukt (New Input System)
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("K werkt via New Input System!");
            CompleteMission(); // Start de missie beloning
        }
    }

    public void CompleteMission()
    {
        // Checkt of alles goed gekoppeld is in Unity
        if (driftManager == null || currencyManager == null)
        {
            Debug.LogError("DriftManager of CurrencyManager is niet gekoppeld!");
            return;
        }

        // Haalt de totale drift score op
        float driftScore = GetTotalScore();

        // Berekent hoeveel coins je krijgt (score * multiplier)
        int reward = Mathf.RoundToInt(driftScore * multiplier);

        // Geeft de coins aan de speler
        currencyManager.AddCoins(reward);

        // Debug info (voor testen)
        Debug.Log("Drift score = " + driftScore);
        Debug.Log("Coins gekregen = " + reward);
    }

    private float GetTotalScore()
    {
        // Haalt de score uit de DriftManager
        return driftManager.totalScore;
    }
}