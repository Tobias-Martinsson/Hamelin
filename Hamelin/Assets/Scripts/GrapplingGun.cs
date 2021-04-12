using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 GrapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public MonoBehaviour controller;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
       
        if (Input.GetKeyDown("e"))
        {
            Debug.Log("e");
            StartGrapple();
        }
        else if (Input.GetKeyUp("e"))
        {
            Debug.Log("E");
            controller.enabled = true;
            StopGrapple();
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        RaycastHit hit;

        Debug.DrawRay(camera.position, camera.forward, Color.red);
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance))
        {
            controller.enabled = false;
            {
                GrapplePoint = hit.point;
                joint = player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = GrapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, GrapplePoint);

                joint.maxDistance = distanceFromPoint * 0.8f;
                joint.minDistance = distanceFromPoint * 0.25f;

                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;
            }

        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, GrapplePoint);
    }
}
