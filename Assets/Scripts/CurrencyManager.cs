using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int coins = 500;

    public bool CanAfford(int amount)
    {
        return coins >= amount;
    }

    public void Spend(int amount)
    {
        coins -= amount;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }
}
