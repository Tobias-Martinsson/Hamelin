using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Tobias Martinsson
[CreateAssetMenu()]
public class PlayerAirborneState : State
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
     
    
      
        if (Player.GroundCheck(Player.point2))
        {
            Debug.Log("Switched to Grounded");
            StateMachine.ChangeState<PlayerGroundedState>();
        }
    }
}
