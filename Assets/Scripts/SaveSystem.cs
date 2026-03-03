using UnityEngine;
using System.IO;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/carSave.json";

    public static void Save(CarUpgradeData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static CarUpgradeData Load()
    {
        if (!File.Exists(path))
        {
            return new CarUpgradeData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<CarUpgradeData>(json);
    }
}
