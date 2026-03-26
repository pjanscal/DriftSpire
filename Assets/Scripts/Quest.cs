using UnityEngine;

public enum QuestType
{
    DriftTime,          // total time 
    DriftScore,         // total score 

    SingleDriftScore,   //  score in one drift
    SingleDriftTime,    //  time in one drift
    MaxMultiplier,      //  reach multiplier
    DriftAngle          //  reach angle
}

[System.Serializable]
public class Quest
{
    public string questName;
    public QuestType questType;

    public float targetValue;   // goal (ex: 3 seconds)
    public float currentValue;  // progress (ex: 1.5 seconds)

    public bool isCompleted;

    public void AddProgress(float amount)
    {
        if (isCompleted) return;

        currentValue += amount;

        if (currentValue >= targetValue)
        {
            isCompleted = true;
            Debug.Log(questName + " completed!");
        }
    }
    public void CheckComplete(float value)
    {
        if (isCompleted) return;

        //  save best attempt
        if (value > currentValue)
        {
            currentValue = value;
        }

        //  check completion
        if (currentValue >= targetValue)
        {
            isCompleted = true;
            Debug.Log(questName = " completed!");
        }
    }
}