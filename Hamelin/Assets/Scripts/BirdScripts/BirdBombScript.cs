using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdBombScript : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Geometry")
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController3D>().setDamageDealt(true);
        }

    }
}