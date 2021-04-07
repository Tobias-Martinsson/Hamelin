using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAttack : State
{
    public float AttackDistance;
    SomeAgent Agent;
    public Transform Player;


    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    }

    public override void RunUpdate()
    {
        Debug.Log("Attack");
        //Agent.Player.GetComponent<PlayerController3D>().velocity += Agent.transform.position.normalized * 0.05f;
        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > AttackDistance)
        {
            StateMachine.ChangeState<AiChasePlayer>();
        }
        
    }
}
