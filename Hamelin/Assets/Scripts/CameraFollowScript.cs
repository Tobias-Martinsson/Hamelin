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

    private void Start()
    {
        
    }
    private void Update()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        Vector3 offset = transform.rotation * cameraOffset;

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        //transform.position = (targetObject.transform.position + offset);

        if (Physics.SphereCast(targetObject.transform.position + new Vector3(0,1,0), 2f, offset + transform.position.normalized, out hitInfo, offset.magnitude, cameraCollisionMask))
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            transform.position = hitInfo.point + hitInfo.normal * 2;

        }
        else
        {
            transform.position = (targetObject.transform.position + offset);
        }

    }


}
