using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    public CurrencyManager currency;
    public TextMeshProUGUI text;

    void Update()
    {
        text.text = "Coins: " + currency.coins;
    }
}
