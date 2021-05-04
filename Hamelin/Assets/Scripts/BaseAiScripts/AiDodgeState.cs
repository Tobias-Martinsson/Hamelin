using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AiDodgeState : State
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
        int random = Random.Range(1, 3);
        switch (random)
        {
            case 1:
                break;
            case 2:
                Agent.NavAgent.velocity += Vector3.left * 2;
                Debug.Log("dashed left");
                break;
            case 3:
                Agent.NavAgent.velocity += Vector3.right * 2;
                Debug.Log("dashed right");
                break;
        }


        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > AttackDistance)
        {
            Debug.Log("Chase");
            StateMachine.ChangeState<AiChasePlayer>();
        }

    }
}