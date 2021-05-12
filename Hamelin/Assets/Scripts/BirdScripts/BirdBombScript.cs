using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Author: Tobias Martinsson
public class BirdBombScript : MonoBehaviour
{

    public float timeLeft = 5.0f;
    public float originalTime;

    private void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Geometry")
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController3D>().SetDamageDealt(true);
        }

    }
}