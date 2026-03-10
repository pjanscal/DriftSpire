using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public CarController car;
    public CurrencyManager currency;

    public int colorPrice = 100;
    public int wheelPrice = 200;
    public int hornPrice = 150;
    public int lightPrice = 250;

    void Start()
    {
        car.upgradeData = SaveSystem.Load();
        car.ApplyUpgrades();
    }

    public void BuyColor(int index)
    {
        if (!currency.CanAfford(colorPrice)) return;

        currency.Spend(colorPrice);

        car.upgradeData.colorIndex = index;
        car.ApplyUpgrades();

        SaveSystem.Save(car.upgradeData);
    }

    public void ToggleHeadlights()
    {
        car.upgradeData.headlightsOn = !car.upgradeData.headlightsOn;

        car.ApplyUpgrades();
        SaveSystem.Save(car.upgradeData);
    }

    public void BuyHorn(int index)
    {
        if (!currency.CanAfford(hornPrice)) return;

        currency.Spend(hornPrice);

        car.upgradeData.hornIndex = index;
        SaveSystem.Save(car.upgradeData);
    }

    public void BuyWheels(int index)
    {
        if (!currency.CanAfford(wheelPrice)) return;

        currency.Spend(wheelPrice);

        car.upgradeData.wheelIndex = index;
        car.ApplyUpgrades();

        SaveSystem.Save(car.upgradeData);
    }

    public void BuyLights(int index)
    {
        if (!currency.CanAfford(lightPrice)) return;

        currency.Spend(lightPrice);

        car.upgradeData.lightIndex = index;
        car.ApplyUpgrades();

        SaveSystem.Save(car.upgradeData);
    }
}