using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public CarController car;
    public CurrencyManager currency;

    void Start()
    {
        car.upgradeData = SaveSystem.Load();
        car.ApplyUpgrades();
    }

    public void SetColor(int index)
    {
        if (!currency.CanAfford(100)) return;

        currency.Spend(100);

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

    public void SetHorn(int index)
    {
        car.upgradeData.hornIndex = index;
        SaveSystem.Save(car.upgradeData);
    }
}
