using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Tim Agélii
//Secondary Author: Tobias Martinsson
public class BugNetController : MonoBehaviour
{
    public new SphereCollider collider;

    private int score = 0;

    void Awake() => collider = GetComponent<SphereCollider>();

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Rat hit");
            GetComponentInParent<PlayerController3D>().SetCatchCheckTrue();
            Destroy(collision.gameObject);
            //Ifall flying enemies
            //      Destroy(collision.gameObject.GetComponentInParent<GameObject>().gameObject);
            //collision.gameObject.GetComponent<Renderer>().enabled = false;
            
            //incresse scorecount by 1.
            AddScore();
        }  
    }

    private void AddScore() => score++;

    public int Score => score;

}
