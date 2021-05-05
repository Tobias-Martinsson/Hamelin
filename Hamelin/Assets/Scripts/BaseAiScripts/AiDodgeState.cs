using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Author: Tobias Martinsson
[CreateAssetMenu()]
public class AiDodgeState : State
{
    SomeAgent Agent;
    int random;

    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);

    }

    private int randomize(int x, int y)
    {
        return Random.Range(x, y);
    }

    public override void RunUpdate()
    {
        random = randomize(1,4);
        Debug.Log(random);
        switch (random)
        {
            case 1:
                Agent.NavAgent.velocity = (Agent.transform.right * 6) + Agent.transform.forward * 6;
                StateMachine.ChangeState<AiChasePlayer>();
                break;
            case 2:
                //Debug.Log("Random is 3. dashed right");
                Agent.NavAgent.velocity = -(Agent.transform.right * 6) + Agent.transform.forward * 6;
                StateMachine.ChangeState<AiChasePlayer>();
                break;
            default:
                StateMachine.ChangeState<AiChasePlayer>();
                break;
        }
    }
}