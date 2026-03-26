using UnityEngine;
using UnityEngine.UI;

public class BuyHandler : MonoBehaviour
{
    public CurrencyManager currencyManager;
    public float cost = 20000f;
    public Button playButton;
    public Canvas canvas;
    public void OnYesClicked()
    {
        if (currencyManager.CanAfford(cost))
        {
            currencyManager.Spend(cost);

            Debug.Log("Level unlocked!");

            playButton.interactable = true;

            canvas.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }
}