using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;

    void OnTriggerEnter(Collider collision)
    {

        Debug.Log("ENTER");
        //input for ladder


        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController3D>().setLadderPointTop(topPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().setLadderPointBottom(bottomPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().setClimbing(true);

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController3D>().setClimbing(false);
            collision.gameObject.GetComponent<PlayerController3D>().setAllowClimb(false);

        }
    }
}

