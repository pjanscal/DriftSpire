using UnityEngine;
using System.IO;

public class CurrencyManager : MonoBehaviour
{
    public float coins;

    string path;

    void Awake()
    {
        path = Application.persistentDataPath + "/coins.json";
        LoadCoins();
    }

    public bool CanAfford(float amount)
    {
        return coins >= amount;
    }

    public void Spend(float amount)
    {
        coins -= amount;
        SaveCoins();
    }

    public void AddCoins(float amount)
    {
        coins += amount;
        SaveCoins();
    }

    void SaveCoins()
    {
        File.WriteAllText(path, coins.ToString());
    }

    void LoadCoins()
    {
        if (File.Exists(path))
        {
            coins = float.Parse(File.ReadAllText(path));
        }
        else
        {
            coins = 500;
        }
    }
}