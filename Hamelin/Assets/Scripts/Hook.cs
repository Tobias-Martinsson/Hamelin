using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Henrik Rudén
public class Hook : MonoBehaviour
{
    public float hookForce = 25f;

    PlayerController3D grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;

    public void Initialize(PlayerController3D grapple, Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        this.grapple = grapple;
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        rigid.AddForce(transform.forward * hookForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positions = new Vector3[]
            {
                transform.position,
                grapple.transform.position
            };

        lineRenderer.SetPositions(positions);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((LayerMask.GetMask("geometry") & 1 << other.gameObject.layer) > 0)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;

            grapple.StartPull();
        }
    }
}
