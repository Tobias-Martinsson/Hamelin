using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdBombScript : MonoBehaviour
{
    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Geometry")
        {
            Destroy(this);
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(scene.name);
        }

    }
}