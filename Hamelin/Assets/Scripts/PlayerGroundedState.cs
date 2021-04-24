using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerGroundedState : State
{
    PlayerController3D Player;

    protected override void Initialize()
    {
        Player = (PlayerController3D)Owner;
        Debug.Assert(Player);
    }

    public override void Enter()
    {
       
    }
    public override void RunUpdate()
    {
      


        if (!Player.GroundCheck(Player.point2))
        {
            Debug.Log("Changed to Airborne");
            StateMachine.ChangeState<PlayerAirborneState>();
        }

    }
}
