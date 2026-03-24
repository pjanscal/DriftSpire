using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class PlayerXPData
{
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;
}

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    public PlayerXPData playerData;

    private string savePath;

    public event Action<int,int> OnXPChanged; // currentXP, xpToNextLevel
    public event Action<int> OnLevelUp; // new level

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/playerXP.json";
            Load();
        }
        else Destroy(gameObject);
    }

    public void AddXP(int amount)
    {
        playerData.currentXP += amount;
        CheckLevelUp();
        Save();
        OnXPChanged?.Invoke(playerData.currentXP, playerData.xpToNextLevel);
    }

    private void CheckLevelUp()
    {
        while(playerData.currentXP >= playerData.xpToNextLevel)
        {
            playerData.currentXP -= playerData.xpToNextLevel;
            playerData.level++;
            playerData.xpToNextLevel = Mathf.RoundToInt(playerData.xpToNextLevel * 1.15f);
            OnLevelUp?.Invoke(playerData.level);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            playerData = JsonUtility.FromJson<PlayerXPData>(json);
        }
        else
        {
            playerData = new PlayerXPData();
            Save();
        }
    }
}