using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Author: Freja Muruganand
public class MainMenu : MonoBehaviour
{
    public GameObject mainMeny;
    public GameObject optionsMeny;
    //public void OnPlay()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    
    //}

    
    
    //When player push the quit button
    public void OnExit()
    {
        Application.Quit();
    }

    //When player push the new game button
    public void OnNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    //When player push the load game button
    public void OnLoadGame()
    {

    }

    // when player push the options buttonm
    public void OnOptions()
    {
        mainMeny.SetActive(false);
        optionsMeny.SetActive(true);

    }

    //when player push the on credits button

    public void OnCredits()
    {

    }
        //when player push the on how to play button
    public void OnHowToPlay()
    {

    }

        //when player is in the options meny and wants to go to main meny
public void OnExitOptions()
    {
        mainMeny.SetActive(true);
        optionsMeny.SetActive(false);

    }
   
}
