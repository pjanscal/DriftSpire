using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public QuestManager questManager;
    public TMP_Text questText;

    void Update()
    {
        string text = "";

        foreach (Quest quest in questManager.activeQuests)
        {
            text += quest.questName + ": " +
                    Mathf.FloorToInt(quest.currentValue) + "/" +
                    quest.targetValue;

            if (quest.isCompleted)
                text += "Completed";

            text += "\n";
        }

        questText.text = text;
    }
}