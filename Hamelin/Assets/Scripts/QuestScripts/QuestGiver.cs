using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public Text enemyAmountText;

    //Activates quest window UI and assigns text to be whatever has been specified.
    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        enemyAmountText.text = quest.enemyAmount.ToString();
    }

    //Code to execute when a quest has been accepted. 
    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;
    }
}