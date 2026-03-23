using UnityEngine;
using System.IO;

public static class SaveSystem
{
    // Pad waar de save file komt
    static string path = Application.persistentDataPath + "/carSave.json";

    public static void Save(CarUpgradeData data)
    {
        // Zet de data om naar JSON (leesbaar formaat)
        string json = JsonUtility.ToJson(data, true);

        // Schrijft het naar een bestand
        File.WriteAllText(path, json);
    }

    public static CarUpgradeData Load()
    {
        // Checkt of er al een save bestaat
        if (!File.Exists(path))
        {
            // Zo niet, maak een nieuwe (lege) data
            return new CarUpgradeData();
        }

        // Leest de JSON uit het bestand
        string json = File.ReadAllText(path);

// JSON slaat alleen simpele dingen op, zoals cijfers of tekst.
// Bijvoorbeeld: "lamp": 2. JSON weet niet dat 2 een rode lamp is.
// Bij het laden leest Unity dat getal terug en mijn script maakt daar weer de rode lamp van.
//
// return geeft het gemaakte object terug aan het script dat deze functie gebruikt.
// Zonder return blijft het object in de functie zitten en kan niemand het gebruiken.
// Een void-functie kan niks teruggeven, daarom mag daar geen return met waarde in staan.

        return JsonUtility.FromJson<CarUpgradeData>(json);
    }
}