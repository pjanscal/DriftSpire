using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> questPool = new List<Quest>();     // ALL quests
    public List<Quest> activeQuests = new List<Quest>();  // CURRENT quests

    public int maxActiveQuests = 2;

    void Start()
    {
        GenerateStartingQuests();
    }

    void GenerateStartingQuests()
    {
        for (int i = 0; i < maxActiveQuests; i++)
        {
            AddRandomQuest();
        }
    }

    void AddRandomQuest()
    {
        if (questPool.Count == 0) return;

        // Maak een lijst van mogelijke quests (zonder duplicates)
        List<Quest> availableQuests = new List<Quest>();

        foreach (Quest quest in questPool)
        {
            bool alreadyActive = false;

            foreach (Quest active in activeQuests)
            {
                if (active.questName == quest.questName)
                {
                    alreadyActive = true;
                    break;
                }
            }

            if (!alreadyActive)
            {
                availableQuests.Add(quest);
            }
        }

        // Als alles al actief is  stop (of reset systeem later)
        if (availableQuests.Count == 0)
        {
            Debug.Log("No more unique quests available!");
            return;
        }

        // Kies random uit beschikbare quests
        Quest randomQuest = availableQuests[Random.Range(0, availableQuests.Count)];

        // Maak kopie
        Quest newQuest = new Quest
        {
            questName = randomQuest.questName,
            questType = randomQuest.questType,
            targetValue = randomQuest.targetValue,
            currentValue = 0,
            isCompleted = false
        };

        activeQuests.Add(newQuest);
    }

    public void AddProgress(QuestType type, float amount)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.questType == type && !quest.isCompleted)
            {
                quest.AddProgress(amount);
            }
        }
    }

    public void CheckQuest(QuestType type, float value)
    {
        foreach (Quest quest in activeQuests)
        {
            if (quest.questType == type && !quest.isCompleted)
            {
                quest.CheckComplete(value);

                if (quest.isCompleted)
                {
                    StartCoroutine(ReplaceQuestWithDelay(quest));
                }
            }
        }
    }

    void ReplaceQuest(int index)
    {
        activeQuests.RemoveAt(index);
        AddRandomQuest();
    }
    IEnumerator ReplaceQuestWithDelay(Quest questToReplace)
    {
        yield return new WaitForSeconds(4f);

        if (activeQuests.Contains(questToReplace))
        {
            activeQuests.Remove(questToReplace);
            AddRandomQuest();
        }
    }
}