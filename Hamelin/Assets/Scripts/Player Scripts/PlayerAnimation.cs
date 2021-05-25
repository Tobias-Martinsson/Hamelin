using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public PlayerController3D player;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        player = player.GetComponent<PlayerController3D>();
    }

    void Update()
    {
        if(player.climbing == true)
        {
            anim.SetBool("Climb", true);
        }
        else
        {
            anim.SetBool("Climb", false);
        }

        /*if(player.health == 0)
        {
            anim.SetBool("Dead", true);
        }
        else
        {
            anim.SetBool("Dead", false);
        }*/
    }
}
