using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform targetObject;

    public Vector3 cameraOffset;
    public float smoothFactor = 0.5f;
    public float mouseSensitivity;
    private RaycastHit hitInfo;
    public float rotationX, rotationY;
    public LayerMask cameraCollisionMask;
    private float cameraRadius = 0.01f;

    private void Start()
    {
        
    }
    private void Update()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * mouseSensitivity;

        rotationX = Mathf.Clamp(rotationX,-89, 89);
       
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraOffset.z += 1f;
            cameraOffset.z = Mathf.Clamp(cameraOffset.z,-8f,-2f);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraOffset.z -= 1f;
            cameraOffset.z = Mathf.Clamp(cameraOffset.z, -8f, -2f);
        }


    }

    private void LateUpdate()
    {
        Vector3 offset = transform.rotation * cameraOffset;
      
        if (Physics.SphereCast(targetObject.transform.position, cameraRadius, offset.normalized, out hitInfo, offset.magnitude, cameraCollisionMask))
        {
            // Ignore camera collision with geometri with geometryCameraIgnore tag
            if (hitInfo.collider != null)
            {
                if (hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("geometryCameraIgnore"))
                {
                    offset = offset.normalized * hitInfo.distance;
                }

            }
            //

            // original before ignore tag
            //offset = offset.normalized * hitInfo.distance;

        }

        transform.position = targetObject.transform.position + offset;
      

    }

}
