using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DynamicFootsteps : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] defaultSteps;
    public AudioClip[] woodSteps;
    public AudioClip[] grassSteps;
    public AudioClip[] carSteps;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PlayDynamicFootstep()
    {

    }
}
