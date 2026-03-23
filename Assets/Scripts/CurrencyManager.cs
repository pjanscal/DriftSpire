using UnityEngine;
using System.IO;

public class CurrencyManager : MonoBehaviour
{
    public float coins; // Houdt bij hoeveel coins de speler heeft

    string path; // Hier slaan we het pad op waar het bestand komt

    void Awake()
    {
        // Dit is de locatie waar Unity bestanden mag opslaan (verschilt per device)
        path = Application.persistentDataPath + "/coins.json";

        // Bij het opstarten laden we de coins
        LoadCoins();
    }

    public bool CanAfford(float amount)
    {
        // Checkt of de speler genoeg coins heeft om iets te kopen
        return coins >= amount;
    }

    public void Spend(float amount)
    {
        // Haalt coins eraf als je iets koopt
        coins -= amount;

        // Daarna meteen opslaan zodat het niet verloren gaat
        SaveCoins();
    }

    public void AddCoins(float amount)
    {
        // Voegt coins toe (bijvoorbeeld na een missie)
        coins += amount;

        // Daarna meteen opslaan
        SaveCoins();
    }

    void SaveCoins()
    {
        // Slaat het aantal coins op in een bestand (als tekst)
        File.WriteAllText(path, coins.ToString());
    }

    void LoadCoins()
    {
        // Checkt of er al een save bestand bestaat
        if (File.Exists(path))
        {
            // Leest het bestand en zet het om naar een float
            coins = float.Parse(File.ReadAllText(path));
        }
        else
        {
            // Als er nog geen save is, begin je met 500 coins
            coins = 500;
        }
    }
}