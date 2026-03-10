using UnityEngine;
using System.IO;

public class CurrencyManager : MonoBehaviour
{
    public int coins;

    string path;

    void Awake()
    {
        path = Application.persistentDataPath + "/coins.json";
        LoadCoins();
    }

    public bool CanAfford(int amount)
    {
        return coins >= amount;
    }

    public void Spend(int amount)
    {
        coins -= amount;
        SaveCoins();
    }

    public void AddCoins(int amount)
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
            coins = int.Parse(File.ReadAllText(path));
        }
        else
        {
            coins = 500;
        }
    }
}