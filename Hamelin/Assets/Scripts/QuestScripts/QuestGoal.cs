using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public int requiredAmount;
    private int currentAmount;

    public BugNetController bugNet;

    void Start()
    {
        currentAmount = 0;
    }

    void Update()
    {
        currentAmount = bugNet.Score;
    }

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }
}