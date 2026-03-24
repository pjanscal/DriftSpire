using UnityEngine;
using TMPro;

public class XPTextUI : MonoBehaviour
{
    public TMP_Text xpText;

    private void Start()
    {
        if(XPManager.Instance != null)
        {
            XPManager.Instance.OnXPChanged += UpdateXPText;
            UpdateXPText(XPManager.Instance.playerData.currentXP, XPManager.Instance.playerData.xpToNextLevel);
        }
    }

    private void UpdateXPText(int currentXP, int xpToNextLevel)
    {
        if(xpText != null)
            xpText.text = $"XP: {currentXP}/{xpToNextLevel}";
    }

    private void OnDestroy()
    {
        if(XPManager.Instance != null)
            XPManager.Instance.OnXPChanged -= UpdateXPText;
    }
}