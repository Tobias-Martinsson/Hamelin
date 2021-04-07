using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAttack : State
{
    public float AttackDistance;

    SomeAgent Agent;

    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    }

    public override void RunUpdate()
    {
        //Debug.Log("Attacking player.");

        if(Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > AttackDistance)
        {
            StateMachine.ChangeState<AiChasePlayer>();
        }
        
    }
}
