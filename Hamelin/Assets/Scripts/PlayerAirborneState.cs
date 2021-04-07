using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
         Player.input = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");
        Player.input = Player.camera.transform.rotation * Player.input;

        Player.velocity *= Mathf.Pow(Player.airResistance, Time.deltaTime);
        Player.transform.position += Player.velocity * Time.deltaTime;

        if (Physics.CapsuleCast(Player.point1, Player.point2, Player.collider.radius * 0.95f, Vector3.down, Player.groundCheckDistance))
        {
            Debug.Log("Switched to Grounded");
            StateMachine.ChangeState<PlayerGroundedState>();
        }
    }
}
