using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerAnimatorUnscaledTime();
        
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

        public void PlayerAnimatorUnscaledTime()
         {
             AnimatorUpdateMode animatorUpdateMode = animator.updateMode;
             animator.updateMode = animatorUpdateMode;
 
             animatorUpdateMode = AnimatorUpdateMode.UnscaledTime;
         }

    private void PauseMenyActivated()
    {
        
        
        PauseGame();
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
        animator.SetTrigger("OpenPauseMenu");
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

    public void quitGameToMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }
    public void PauseMenyDeactivated()
    {
        animator.SetTrigger("ClosePauseMenu");
        ResumeGame();
        pauseMeny.SetActive(false);
        //activate camera
        //Get mouse back 
        activateMouse(false);
        otherUIActivation(true);


    }

    public void restartLevel()
    {
        PlayerPrefs.SetInt("loaded", 0);
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        animator.SetTrigger("OpenHowTo");
        animator.ResetTrigger("OpenPauseMenu");
        

    }

    public void optionsMenu(){
        animator.SetTrigger("OpenOptions");
        animator.ResetTrigger("OpenPauseMenu");
    }

    public void backToMenu(){
        animator.SetTrigger("BackToMenu");
        
    }




    
}
