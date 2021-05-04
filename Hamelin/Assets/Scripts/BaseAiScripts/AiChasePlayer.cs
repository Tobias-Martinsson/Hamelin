using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiChasePlayer : State
{
    SomeAgent Agent;
    public float Speed;
    public float AttackDistance;
    public float timeLeft = 30;
    private float originalTime;

    protected override void Initialize()
    {
        
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    }

    public override void Enter()
    {
        originalTime = timeLeft;
        Agent.NavAgent.speed = Speed;
    }

    public override void RunUpdate()
    {
        Agent.NavAgent.SetDestination(Agent.PlayerPosition);
        timeLeft -= Time.deltaTime;
        Debug.Log(timeLeft);
        if(timeLeft < 0)
        {
            if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) < 10f && Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > 9f)
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

        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) <= AttackDistance)
        {
            Debug.Log("Attack");
            StateMachine.ChangeState<AiAttack>();
        }

    }
}
