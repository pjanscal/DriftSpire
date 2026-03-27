using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public DriftControllerFixed car;
    public CurrencyManager currency;

    [Header("Button Labels")]
    public TextMeshProUGUI engineLabel;
    public TextMeshProUGUI brakesLabel;
    public TextMeshProUGUI handlingLabel;

    [Header("Upgrade Costs")]
    public int engineUpgradeCost = 300;
    public int brakesUpgradeCost = 250;
    public int handlingUpgradeCost = 200;

    [Header("Buttons")]
    public Button engineButton;
    public Button brakesButton;
    public Button handlingButton;

    public Color normalColor = Color.white;
    public Color maxedColor = Color.red;

    private const int maxLevel = 3;

    void Start()
    {
        car.upgradeData = SaveSystem.Load();
        car.ApplyUpgrades();
        RefreshUI();
    }

    void RefreshUI()
    {
        engineLabel.text = "Engine\nLevel " + car.upgradeData.engineLevel + " / " + maxLevel;
        brakesLabel.text = "Brakes\nLevel " + car.upgradeData.brakesLevel + " / " + maxLevel;
        handlingLabel.text = "Handling\nLevel " + car.upgradeData.handlingLevel + " / " + maxLevel;

        engineButton.interactable = car.upgradeData.engineLevel < maxLevel;
        brakesButton.interactable = car.upgradeData.brakesLevel < maxLevel;
        handlingButton.interactable = car.upgradeData.handlingLevel < maxLevel;
    }

    public void BuyEngineUpgrade()
    {
        if (car.upgradeData.engineLevel >= maxLevel) return;
        if (!currency.CanAfford(engineUpgradeCost)) return;
        currency.Spend(engineUpgradeCost);
        car.upgradeData.engineLevel++;
        car.ApplyUpgrades();
        SaveSystem.Save(car.upgradeData);
    }

    public void BuyBrakesUpgrade()
    {
        if (car.upgradeData.brakesLevel >= maxLevel) return;
        if (!currency.CanAfford(brakesUpgradeCost)) return;
        currency.Spend(brakesUpgradeCost);
        car.upgradeData.brakesLevel++;
        car.ApplyUpgrades();
        SaveSystem.Save(car.upgradeData);
    }

    public void BuyHandlingUpgrade()
    {
        if (car.upgradeData.handlingLevel >= maxLevel) return;
        if (!currency.CanAfford(handlingUpgradeCost)) return;
        currency.Spend(handlingUpgradeCost);
        car.upgradeData.handlingLevel++;
        car.ApplyUpgrades();
        SaveSystem.Save(car.upgradeData);
    }
}