using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main author: Tim Agélii
public class LadderClimb : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;
    public Transform endPoint;
    void OnTriggerEnter(Collider collision)
    {

        Debug.Log("ENTER");
        //input for ladder


        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController3D>().setLadderPointTop(topPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().setLadderPointBottom(bottomPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().setLadderPointEnd(endPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().setClimbReady(true);

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController3D>().setClimbing(false);
 
        }
    }
}

