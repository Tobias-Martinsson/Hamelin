using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugNetController : MonoBehaviour
{
    public SphereCollider collider;
    public RaycastHit hit;
    public float downwardCheckDistance = 1f;
    void Awake() => collider = GetComponent<SphereCollider>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Physics.SphereCast(transform.position, collider.radius, Vector3.down, out hit, downwardCheckDistance))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Renderer>().enabled = false;
                Destroy(hit.transform.gameObject);
            }
                

        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Enemy")
        {
            
            Destroy(collision.gameObject);
            Destroy(collision.transform.parent.gameObject);
            //collision.gameObject.GetComponent<Renderer>().enabled = false;
        }
        
    }
}
