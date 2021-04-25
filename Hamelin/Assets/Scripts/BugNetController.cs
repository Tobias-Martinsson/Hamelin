using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugNetController : MonoBehaviour
{
    public SphereCollider collider;
    public RaycastHit hit;
    public float downwardCheckDistance = 1f;

    private int score = 0;

    void Awake() => collider = GetComponent<SphereCollider>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            //Ifall flying enemies
            //      Destroy(collision.gameObject.GetComponentInParent<GameObject>().gameObject);
            //collision.gameObject.GetComponent<Renderer>().enabled = false;
            
            //incresse scorecount by 1.
            AddScore();
        }
        

        
    }

    private void AddScore() => score++;

    public int Score => score;

}
