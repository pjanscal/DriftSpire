using UnityEngine;
using UnityEngine.UI;

public class BuyHandler : MonoBehaviour
{
    public CurrencyManager currencyManager;
    public float cost = 20000f;
    public int levelID;
    public Button playButton;

    public void OnYesClicked()
    {
        if (currencyManager.CanAfford(cost))
        {
            currencyManager.Spend(cost);
            PlayerPrefs.SetInt("LevelUnlocked_" + levelID, 1);

            Debug.Log("Level " + levelID + " unlocked!");

            playButton.interactable = true; // ✅ FIX
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }
}