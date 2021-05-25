using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Author: Freja Muruganand
public class MainMenu : MonoBehaviour
{
    public GameObject mainMeny;
    public GameObject optionsMeny;
    public void OnPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        mainMeny.SetActive(true);
        optionsMeny.SetActive(false);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnNewGame()
    {

    }
    public void OnLoadGame()
    {

    }
    public void OnOptions()
    {
        mainMeny.SetActive(false);
        optionsMeny.SetActive(true);

    }

    public void OnCredits()
    {

    }
    public void OnHowToPlay()
    {

    }
public void OnExitOptions()
    {
        mainMeny.SetActive(true);
        optionsMeny.SetActive(false);

    }
   
}
