using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Main Author: Freja Muruganand
public class DelayedRepeat : MonoBehaviour
{
    private AudioSource source;

    private float minDelay = 1.0f;
    private float maxDelay = 8.0f;

    private float minVol = 0.5f;
    private float maxVol = 1.0f;

    private float minPitch = 0.9f;
    private float maxPitch = 1.1f;

    public AudioClip[] clips;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            source.volume = Random.Range(minVol, maxVol);
            source.pitch = Random.Range(minPitch, maxPitch);

            int clipIndex = Random.Range(1, clips.Length);
            AudioClip clip = clips[clipIndex];
            source.clip = clip;

            float delay = Random.Range(minDelay, maxDelay);
            source.PlayDelayed(delay);

            clips[clipIndex] = clips[0];
            clips[0] = clip;
        }
    }
}
