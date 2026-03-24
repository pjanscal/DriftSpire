using UnityEngine;

public class XPReceiver : MonoBehaviour
{
    public int xpPerDriftPoint = 1; // hoeveel XP per drift score punt

    public void GiveXPFromDrift(int driftScore)
    {
        if(XPManager.Instance != null)
        {
            XPManager.Instance.AddXP(driftScore * xpPerDriftPoint);
        }
    }
}