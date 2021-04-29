using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiBirdChasePlayer : State
{
    SomeAgent Agent;
    public float Speed;
    public float AttackDistance;
    public float timeLeft;
    public float originalTime;
    public GameObject myPrefab;
    float timer = 0;

    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    }

    public override void Enter()
    {
        Agent.NavAgent.speed = Speed;
        originalTime = timeLeft;
    }

    public override void RunUpdate()
    {
        Agent.NavAgent.SetDestination(Agent.PlayerPosition);
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 && Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) < AttackDistance)
        {
            Destroy(Instantiate(myPrefab, new Vector3(Agent.transform.position.x, Agent.transform.position.y, Agent.transform.position.z), Quaternion.identity), 5f);

            timeLeft = originalTime;

        }

        if (Physics.Linecast(Agent.transform.position, Agent.PlayerPosition, Agent.CollisionLayer))
        {
            Debug.Log("Patrol");
            StateMachine.ChangeState<AiBirdPatrolState>();
        }

        /*
        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) <= AttackDistance)
        {
            Debug.Log("Attack");
            StateMachine.ChangeState<AiBirdAttack>();
        }
        */

    }
}
