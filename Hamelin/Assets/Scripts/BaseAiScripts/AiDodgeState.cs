using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class AiDodgeState : State
{
    int random;
    SomeAgent Agent;

    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    }

    public override void RunUpdate()
    {
            Vector3 tempVelocity = Agent.NavAgent.velocity;
            random = Random.Range(1, 3);
            switch (random)
            {
                case 1:
                    Debug.Log("Random is 1. No dodging. ");
                    StateMachine.ChangeState<AiChasePlayer>();
                    break;
                case 2:
                    tempVelocity.x += 10f;
                    Debug.Log("Random is 2. dashed left");
                    Agent.NavAgent.velocity = tempVelocity;
                    StateMachine.ChangeState<AiChasePlayer>();
                    break;
                case 3:
                    tempVelocity.x += -10f;
                    Agent.NavAgent.velocity = tempVelocity;
                    Debug.Log("Random is 3. dashed right");
                    StateMachine.ChangeState<AiChasePlayer>();
                    break;
            }
    }
}