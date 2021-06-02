using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Author: Freja Muruganand
//Supporting Author My Karlén
public class MainMenu : MonoBehaviour
{
    public GameObject mainMeny;
    public GameObject optionsMeny;
    public Animator animator;

    public AudioClip clip;
    private AudioSource source;
    private float minPitch = 0.9f;
    private float maxPitch = 1.2f;

    private int savedScene;

    
    //public void OnPlay()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    //}

    private void Start()
    {
        Cursor.visible = true;
        PlayerData data = SaveSystem.LoadPlayer();
        savedScene = data.currentScene;
        animator.SetBool("PushedEnter", false);

        source = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
         
            SceneManager.LoadScene(savedScene);
        }
        if (Input.GetKeyDown(KeyCode.Return)){
             animator.SetTrigger("PushedEnter");

        }
    }

    //When player push the quit button
    public void OnExit()
    {
        Application.Quit();
    }

    //When player push the new game button
    public void OnNewGame()
    {
        PlayerPrefs.SetInt("loaded", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    //When player push the load game button
    public void OnLoadGame()
    {
        SceneManager.LoadScene(savedScene);
    }

    // when player push the options buttonm
    public void OnOptions()
    {
        //mainMeny.SetActive(false);
        //optionsMeny.SetActive(true);
        animator.SetTrigger("OptionsMenu");
        animator.ResetTrigger("BackToMain");
        animator.ResetTrigger("HowToPlayMenu");
        

    }

    

    
        //when player push the on how to play button
    public void OnHowToPlay()
    {
        animator.SetTrigger("HowToPlayMenu");


    }

    public void OnExitHowToPlay()
    {
        animator.SetTrigger("BackToMain");
        animator.ResetTrigger("HowToPlayMenu");
        animator.ResetTrigger("OptionsMenu");
    }


    

        //when player is in the options meny and wants to go to main meny
    public void OnExitOptions()
    {
       // mainMeny.SetActive(true);
       // optionsMeny.SetActive(false);
       animator.SetTrigger("BackToMain");
       animator.ResetTrigger("OptionsMenu");
       animator.ResetTrigger("HowToPlayMenu");

    }

    public void PlayButtonSound()
    {
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(clip);
    }

    //when player push the on credits button
    public void OnCredits()
    {
       // mainMeny.SetActive(true);
       // optionsMeny.SetActive(false);
       animator.SetTrigger("CreditsMenu");
       animator.ResetTrigger("OptionsMenu");
       animator.ResetTrigger("HowToPlayMenu");

    }

   
}
