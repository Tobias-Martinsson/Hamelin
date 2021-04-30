using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public int requiredAmount;
    private int currentAmount;

    //BugNetController is accessed so that the score (amount of enemies caught) can be accessed
    public BugNetController bugNet;

    void Start()
    {
        currentAmount = 0;
    }

    void Update()
    {
        currentAmount = bugNet.Score;
    }

    //Bool to check if enough enemies have been caught, when it returns true the quest is completed
    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }
}