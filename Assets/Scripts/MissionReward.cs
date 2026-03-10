using UnityEngine;
using UnityEngine.InputSystem; // BELANGRIJK

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
        float driftScore = GetTotalScore();
        int reward = Mathf.RoundToInt(driftScore * multiplier);

        currencyManager.AddCoins(reward);

        Debug.Log("Drift score = " + driftScore);
        Debug.Log("Coins gekregen = " + reward);
    }

    private float GetTotalScore()
    {
        var field = typeof(Driftmanager).GetField("totalScore",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        return (float)field.GetValue(driftManager);
    }
}
