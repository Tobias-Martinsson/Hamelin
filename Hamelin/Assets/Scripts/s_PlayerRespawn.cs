using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_PlayerRespawn : MonoBehaviour
{

    PlayerController3D player;

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
