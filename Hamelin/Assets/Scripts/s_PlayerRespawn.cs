using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Den h�r koden �r t�nkt att vara en respawn position som f�ljer efter spelaren s� l�nge den vidg�r marken.
public class s_PlayerRespawn : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 1f;
    public Vector3 velosity = Vector3.zero;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosistion = target.TransformPoint(new Vector3(0, 5, 0));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosistion, ref velosity, smoothTime);
    }
}
