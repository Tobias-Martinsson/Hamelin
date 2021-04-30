using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public int enemyAmount;

    public bool isActive;

    public QuestGoal goal;
    public GameObject levelTransition;

    public void Complete()
    {
        isActive = false;
        levelTransition.SetActive(true);
        Debug.Log(title + " was completed");
    }
}