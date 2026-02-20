using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
    public int money = 0;

    public int moneyRewardOnLevelUp = 50;

    public event Action<int,int> OnStatsChanged; // (money, currentXP)
    public event Action<int> OnLevelUp; // new level

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else Destroy(gameObject);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        CheckLevelUp();
        Save();
        OnStatsChanged?.Invoke(money, currentXP);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Save();
        OnStatsChanged?.Invoke(money, currentXP);
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;
        money -= amount;
        Save();
        OnStatsChanged?.Invoke(money, currentXP);
        return true;
    }

    private void CheckLevelUp()
    {
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            level++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.15f);
            AddMoney(moneyRewardOnLevelUp);
            OnLevelUp?.Invoke(level);
        }
    }

    // PlayerPrefs keys
    const string K_LEVEL = "player_level";
    const string K_XP = "player_xp";
    const string K_XP_NEXT = "player_xp_to_next";
    const string K_MONEY = "player_money";

    public void Save()
    {
        PlayerPrefs.SetInt(K_LEVEL, level);
        PlayerPrefs.SetInt(K_XP, currentXP);
        PlayerPrefs.SetInt(K_XP_NEXT, xpToNextLevel);
        PlayerPrefs.SetInt(K_MONEY, money);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(K_LEVEL))
        {
            level = PlayerPrefs.GetInt(K_LEVEL);
            currentXP = PlayerPrefs.GetInt(K_XP);
            xpToNextLevel = PlayerPrefs.GetInt(K_XP_NEXT);
            money = PlayerPrefs.GetInt(K_MONEY);
        }
        else Save();
    }
}
