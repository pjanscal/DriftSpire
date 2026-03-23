using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public CarController car; // De auto die we upgraden
    public CurrencyManager currency; // Het geld systeem

    // 💰 Prijzen van upgrades
    public int colorPrice = 100;
    public int wheelPrice = 200;
    public int hornPrice = 150;
    public int lightPrice = 250;

    void Start()
    {
        // Laadt de opgeslagen upgrades
        car.upgradeData = SaveSystem.Load();

        // Past ze meteen toe op de auto
        car.ApplyUpgrades();
    }

    public void BuyColor(int index)
    {
        // Checkt of je genoeg geld hebt
        if (!currency.CanAfford(colorPrice)) return;

        // Haalt geld eraf
        currency.Spend(colorPrice);

        // Zet de nieuwe kleur
        car.upgradeData.colorIndex = index;

        // Update de auto visueel
        car.ApplyUpgrades();

        // Sla op
        SaveSystem.Save(car.upgradeData);
    }

    public void ToggleHeadlights()
    {
        // Zet koplampen aan/uit (true/false switch)
        car.upgradeData.headlightsOn = !car.upgradeData.headlightsOn;

        car.ApplyUpgrades();
        SaveSystem.Save(car.upgradeData);
    }

    public void BuyHorn(int index)
    {
        if (!currency.CanAfford(hornPrice)) return;

        currency.Spend(hornPrice);

        // Zet nieuwe horn
        car.upgradeData.hornIndex = index;

        // Geen ApplyUpgrades nodig (want geluid verandert pas bij afspelen)
        SaveSystem.Save(car.upgradeData);
    }

    public void BuyWheels(int index)
    {
        if (!currency.CanAfford(wheelPrice)) return;

        currency.Spend(wheelPrice);

        // Zet nieuwe wielen
        car.upgradeData.wheelIndex = index;

        car.ApplyUpgrades();
        SaveSystem.Save(car.upgradeData);
    }

    public void BuyLights(int index)
    {
        if (!currency.CanAfford(lightPrice)) return;

        currency.Spend(lightPrice);

        // Zet nieuwe lampen
        car.upgradeData.lightIndex = index;

        car.ApplyUpgrades();
        SaveSystem.Save(car.upgradeData);
    }
}