using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Main Author: Tim Agélii
//Secondary Author: Tobias Martinsson
public class BugNetController : MonoBehaviour
{
    public new SphereCollider collider;

    public AudioClip catchSound;

    private AudioSource source;

    private float minPitch = 0.7f;
    private float maxPitch = 0.9f;

    private int score = 0;

    void Awake() => collider = GetComponent<SphereCollider>();

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
     
        if(collision.gameObject.tag == "Enemy")
        {
        
            GetComponentInParent<PlayerController3D>().SetCatchCheckTrue();
            Destroy(collision.gameObject);
            //Ifall flying enemies
            //      Destroy(collision.gameObject.GetComponentInParent<GameObject>().gameObject);
            //collision.gameObject.GetComponent<Renderer>().enabled = false;
            
            //incresse scorecount by 1.
            AddScore();
            PlayCatchSound();
        }  
    }

    public int getScore()
    {
        return score;
    }

    public void setScore(int savedScore)
    {
        score = savedScore;
    }

    private void AddScore() => score++;

    private void PlayCatchSound()
    {
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(catchSound);
    }

    public int Score => score;

}
