using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main author: Tim Agélii
public class LadderClimb : MonoBehaviour
{
    public Transform topPoint;
    public Transform bottomPoint;
    public Transform endPoint;
    public Transform ladderTransform;
   
   

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float yPos = collision.gameObject.transform.position.y;
    
      
        

            if (Mathf.Abs(yPos - topPoint.position.y) <= (Mathf.Abs(yPos - bottomPoint.position.y)))
            {
                collision.gameObject.GetComponent<PlayerController3D>().SetLadderStartPointBottom(false);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController3D>().SetLadderStartPointBottom(true);
            }

        }
    }
  
       
        void OnTriggerEnter(Collider collision)
        {
        
        //input for ladder

        if (collision.gameObject.tag == "Player")
        {

            collision.gameObject.GetComponent<PlayerController3D>().SetLadderPointTop(topPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().SetLadderPointBottom(bottomPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().SetLadderPointEnd(endPoint.position);
            collision.gameObject.GetComponent<PlayerController3D>().SetClimbReady(true);

            
            collision.gameObject.GetComponent<PlayerController3D>().SetLadderRotation(ladderTransform.rotation.eulerAngles.y);
            Debug.Log(ladderTransform.rotation.eulerAngles.y);
        
        
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

