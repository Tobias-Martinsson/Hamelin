using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Main author: Andreas Scherman
//Secondary author: Freja Muruganand
public class BobController : MonoBehaviour
{
    public AudioClip attackSound;
    public AudioClip[] ladderSounds;

    private Animator anim;
    private AudioSource source;

    private float speed;
    private float direction;

    private float minPitch = 0.7f;
    private float maxPitch = 1.2f;

    void Start()
    {       
        anim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        speed = Input.GetAxis("Vertical");
        direction = Input.GetAxis("Vertical") + Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", speed);
        anim.SetFloat("Direction", direction);

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
        }
        /*if (Input.GetButtonUp("Fire1"))
            anim.SetBool("Attack", false);*/
        if (Input.GetButtonDown("Fire2"))
            anim.SetTrigger("Parry");
        if (Input.GetButtonDown("Jump"))
            anim.SetTrigger("Jump");
        if (Input.GetButtonDown("Fire3"))
            anim.SetTrigger("Taunt");
    }

    public void PlayAttackSound()
    {
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(attackSound);
    }

    public void PlayLadderSound()
    {
        int clipIndex = Random.Range(1, ladderSounds.Length);
        AudioClip clip = ladderSounds[clipIndex];
        source.PlayOneShot(clip);
        ladderSounds[clipIndex] = ladderSounds[0];
        ladderSounds[0] = clip;
    }
}
