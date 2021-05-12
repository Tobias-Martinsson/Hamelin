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
            collision.gameObject.GetComponent<PlayerController3D>().SetLadderPointTop(topPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().SetLadderPointBottom(bottomPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().SetLadderPointEnd(endPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().SetClimbReady(true);

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController3D>().SetClimbing(false);
            collision.gameObject.GetComponent<PlayerController3D>().SetClimbReady(false);
        }
    }
}

