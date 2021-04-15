using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobController : MonoBehaviour
{
    private Animator anim;

    private float speed;
    private float direction;


    // Start is called before the first frame update
    void Start()
    {       
        anim = gameObject.GetComponent<Animator>();      
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = Input.GetAxis("Vertical");
        direction = Input.GetAxis("Vertical") + Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", speed);
        anim.SetFloat("Direction", direction);

        if (Input.GetButtonDown("Fire1"))
            anim.SetTrigger("Attack");
        if (Input.GetButtonDown("Fire2"))
            anim.SetTrigger("Parry");
        if (Input.GetButtonDown("Jump"))
            anim.SetTrigger("Jump");
        if (Input.GetButtonDown("Fire3"))
            anim.SetTrigger("Taunt");

    }
}
