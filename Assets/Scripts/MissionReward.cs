using UnityEngine;
using UnityEngine.InputSystem;

public class MissionReward : MonoBehaviour
{
    public Driftmanager driftManager;
    public CurrencyManager currencyManager;

    public float multiplier = 1.5f;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("K werkt via New Input System!");
            CompleteMission();
        }
    }

    public void CompleteMission()
    {
        if (driftManager == null || currencyManager == null)
        {
            Debug.LogError("DriftManager of CurrencyManager is niet gekoppeld!");
            return;
        }

        float driftScore = GetTotalScore();
        int reward = Mathf.RoundToInt(driftScore * multiplier);

        currencyManager.AddCoins(reward);

        Debug.Log("Drift score = " + driftScore);
        Debug.Log("Coins gekregen = " + reward);
    }

    private float GetTotalScore()
    {
        return driftManager.totalScore;
    }
}