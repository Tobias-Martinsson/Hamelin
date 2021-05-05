using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Tobias Martinsson
[CreateAssetMenu()]
public class AiChasePlayer : State
{
    SomeAgent Agent;
    public float Speed;
    public float AttackDistance;
    private float timeLeft = 1;
    private float originalTime;
    private float dodgeRange = 6f;


    protected override void Initialize()
    {
        originalTime = timeLeft;
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    }

    public override void Enter()
    {
        Agent.NavAgent.speed = Speed;
    }

    public override void RunUpdate()    
    {
        Agent.NavAgent.SetDestination(Agent.PlayerPosition);
        //Debug.Log(timeLeft);
        timeLeft -= Time.deltaTime;
        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) <= AttackDistance)
        {
            Debug.Log("Attack");
            StateMachine.ChangeState<AiAttack>();
        }
        if (timeLeft < 0)
        {
            if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) < dodgeRange && Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > dodgeRange - 1)
            {
                Debug.Log("Dodging");
                timeLeft = originalTime;
                StateMachine.ChangeState<AiDodgeState>();
            }
        }
            

        if (Physics.Linecast(Agent.transform.position, Agent.PlayerPosition, Agent.CollisionLayer))
        {
            Debug.Log("Patrol");
            StateMachine.ChangeState<AiPatrolState>();
        }

        

    }
}
