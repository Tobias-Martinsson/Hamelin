using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Freja Muruganand
//Secondary Author: Tobias Martinsson
[System.Serializable]
public class QuestGoal 
{
    public int requiredAmount;
    public int currentAmount;

    //BugNetController is accessed so that the score (amount of enemies caught) can be accessed
    public BugNetController bugNet;

    //Bool to check if enough enemies have been caught, when it returns true the quest is completed
    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }
}