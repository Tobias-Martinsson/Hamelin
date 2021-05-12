using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DynamicFootsteps : MonoBehaviour
{
    private AudioSource source;

    private string colliderType;

    public AudioClip[] defaultSteps;
    public AudioClip[] woodSteps;
    public AudioClip[] grassSteps;
    public AudioClip[] carSteps;

    void Start()
    {
        source = GetComponent<AudioSource>();
        colliderType = "";
    }

    public void PlayDynamicFootstep(string colType)
    {
        colliderType = colType;

        if (!source.isPlaying)
        {
            switch (colliderType)
            {
                case "Wood":
                    int clipIndex1 = Random.Range(1, woodSteps.Length);
                    AudioClip clip1 = woodSteps[clipIndex1];
                    source.PlayOneShot(clip1);
                    woodSteps[clipIndex1] = woodSteps[0];
                    woodSteps[0] = clip1;
                    break;
                case "Grass":
                    int clipIndex2 = Random.Range(1, grassSteps.Length);
                    AudioClip clip2 = grassSteps[clipIndex2];
                    source.PlayOneShot(clip2);
                    grassSteps[clipIndex2] = grassSteps[0];
                    grassSteps[0] = clip2;
                    break;
                case "Car":
                    int clipIndex3 = Random.Range(1, carSteps.Length);
                    AudioClip clip3 = carSteps[clipIndex3];
                    source.PlayOneShot(clip3);
                    carSteps[clipIndex3] = carSteps[0];
                    carSteps[0] = clip3;
                    break;
                default:
                    int clipIndex4 = Random.Range(1, defaultSteps.Length);
                    AudioClip clip4 = defaultSteps[clipIndex4];
                    source.PlayOneShot(clip4);
                    defaultSteps[clipIndex4] = defaultSteps[0];
                    defaultSteps[0] = clip4;
                    break;
            }
        }       
    }
}
