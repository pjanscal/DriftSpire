using UnityEngine;

public class MissionReward : MonoBehaviour
{
    public int xpReward = 30;
    public int moneyReward = 100;

    public void CompleteMission()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.AddXP(xpReward);
        PlayerStats.Instance.AddMoney(moneyReward);
        Debug.Log($"Missie voltooid: +{xpReward} XP, +{moneyReward} geld");
    }
}
