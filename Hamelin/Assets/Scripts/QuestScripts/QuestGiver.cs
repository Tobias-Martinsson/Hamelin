using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Main Author: Freja Muruganand
//Secondary Author: Tobias Martinsson
public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public QuestGoal questGoal;
    public BugNetController bugNet;

    public GameObject questWindow;
    public GameObject logWindow;

    public Text titleText;
    public Text dialogueText;
    public Text descriptionText;
    public Text logText;
    public Text instructionText;

    public Animator anim;

    //Activates quest window UI and assigns text to be whatever has been specified.
    void Start()
    {
        if(anim != null)
        {
            anim = anim.GetComponent<Animator>();
        }
        
        questGoal.currentAmount = 0;
        questWindow.SetActive(true);
        logWindow.SetActive(false);
        titleText.text = quest.title;
        dialogueText.text = quest.dialogue;
        descriptionText.text = quest.description;
        instructionText.text = quest.instructions;
        logText.text = "";
    }

    //Code to execute when a quest has been accepted. 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            questWindow.SetActive(false);
            quest.isActive = true;

            SetQuestLog();
        }
      
            if (questGoal.currentAmount == 9 && anim != null)
            {
                anim.SetBool("IsParked", true);
            }
        
        

        questGoal.currentAmount = bugNet.Score;
        quest.QuestCompleted();
        logText.text = "Caught " + bugNet.Score.ToString() + " / " + quest.enemyAmount.ToString() + " pests.";
    }

    private void SetQuestLog()
    {
        logWindow.SetActive(true);
    }
}