using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string dialogue;
    public string description;
    //The amount of enemies that are required to be caught
    public int enemyAmount;

    public bool isActive;

    public QuestGoal goal;
    public GameObject levelTransition;

    void Update()
    {
        if(goal.IsReached() == true) 
        {
            isActive = false;
            levelTransition.SetActive(true);
            Debug.Log(title + " was completed");
        }
    }
}