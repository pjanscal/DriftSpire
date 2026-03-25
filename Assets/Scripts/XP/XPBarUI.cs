using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    public Slider xpSlider;
    public TMP_Text xpText;
    private void Start()
    {
        if (XPManager.Instance != null)
        {
            XPManager.Instance.OnXPChanged += UpdateXPBar;
            UpdateXPBar(XPManager.Instance.playerData.currentXP, XPManager.Instance.playerData.xpToNextLevel);
        }
    }

    private void UpdateXPBar(int currentXP, int xpToNextLevel)
    {
        if(xpSlider != null)
            xpSlider.value = (float)currentXP / xpToNextLevel;
        if(xpText != null)
            xpText.text = "LVL " + XPManager.Instance.playerData.level + 
              " | " + currentXP + " / " + xpToNextLevel + " XP";
    }

    private void OnDestroy()
    {
        if(XPManager.Instance != null)
            XPManager.Instance.OnXPChanged -= UpdateXPBar;
    }
}