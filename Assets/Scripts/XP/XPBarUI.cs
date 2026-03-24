using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    public Slider xpSlider;

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
    }

    private void OnDestroy()
    {
        if(XPManager.Instance != null)
            XPManager.Instance.OnXPChanged -= UpdateXPBar;
    }
}