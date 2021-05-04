using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RandomStart : MonoBehaviour
{
    private AudioSource source;

    public AudioClip clip;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;

        source.time = Random.Range(0, clip.length);
        source.Play();
    }

}
