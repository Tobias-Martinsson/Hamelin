using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMeny : MonoBehaviour
{
    public GameObject pauseMeny;
    public GameObject otherUiElements;
    public GameObject convoPanel;
    public GameObject popupPanel;
    public GameObject questPanel;
    public GameObject hTPPanel;
    public bool conversationPanelActivation;
    public bool popupPanelActivation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (pauseMeny.activeInHierarchy)
            {
                PauseMenyDeactivated();

            }
            else{
                PauseMenyActivated();
            }
            
        }
    }

    private void PauseMenyActivated()
    {
        PauseGame();
        pauseMeny.SetActive(true);
        //deactivate camera
        //Get mouse back 
        activateMouse(true);
        if(convoPanel.activeInHierarchy){
            conversationPanelActivation = true;
        } else{
            conversationPanelActivation = false;
        }

        if(popupPanel.activeInHierarchy){
            popupPanelActivation = true;
        } else {
            popupPanelActivation = false;
        }
        otherUIActivation(false);


    }

    private void PauseGame ()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame ()
    {
        Time.timeScale = 1;
    }
    private void activateMouse(bool activation){
        //Set Cursor to not be visible
        Cursor.visible = activation;
    }
    private void PauseMenyDeactivated()
    {
        ResumeGame();
        pauseMeny.SetActive(false);
        //activate camera
        //Get mouse back 
        activateMouse(false);
        otherUIActivation(true);


    }
    private void otherUIActivation(bool activation){
        if (activation){
            if(conversationPanelActivation){
                convoPanel.SetActive(true);
            }
            if(popupPanelActivation){
                popupPanel.SetActive(true);
            }
            questPanel.SetActive(true);


        } else {
            convoPanel.SetActive(false);
            popupPanel.SetActive(false);
            questPanel.SetActive(false);

        }

    }

    public void howToPlay(){
        hTPPanel.SetActive(true);
        pauseMeny.SetActive(false);

    }

    public void backToMenu(){
        hTPPanel.SetActive(false);
        pauseMeny.SetActive(true);
    }




    
}
