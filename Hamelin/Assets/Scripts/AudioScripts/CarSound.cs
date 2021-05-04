using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSound : MonoBehaviour
{
    public AudioMixer masterMix;
    public BugNetController bugNet;

    //How long the cars can be heard
    public float waitTime = 6f;

    private bool turnedOn;
    private IEnumerator coroutine;

    //How many critters have to be caught before the car animation starts
    private int requiredScore = 9;

    void Start()
    {
        bugNet = bugNet.GetComponent<BugNetController>();
        turnedOn = false;

        masterMix.SetFloat("carVolume", -80);
    }

    void Update()
    {
        if (turnedOn == false && bugNet.Score == requiredScore)
        {
            coroutine = HearCars();
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator HearCars()
    {
        Debug.Log("HearCars");
        masterMix.SetFloat("carVolume", 0);

        yield return new WaitForSeconds(waitTime);

        masterMix.SetFloat("carVolume", -80);

        turnedOn = true;
    }
}
