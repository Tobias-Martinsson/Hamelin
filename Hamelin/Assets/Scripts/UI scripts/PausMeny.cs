using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMeny : MonoBehaviour
{
    public GameObject pauseMeny;
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
    public void PauseMenyDeactivated()
    {
        ResumeGame();
        pauseMeny.SetActive(false);
        //activate camera
        //Get mouse back 
        activateMouse(false);


    }

    
}
