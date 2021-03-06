using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Freja Muruganand
//Secondary Author: Tobias Martinsson
[System.Serializable]
public class Quest
{
    public string title;
    public string dialogue;
    public string description;
    public string instructions;
    public QuestGiver questGiver;
    //The amount of enemies that are required to be caught
    public int enemyAmount;

    public bool isActive;

    public GameObject levelTransition;

    public void QuestCompleted()
    {
        if(questGiver.questGoal.IsReached() == true) 
        {
            isActive = false;
            levelTransition.SetActive(true);
            Debug.Log(title + " was completed");
        }
    }
}