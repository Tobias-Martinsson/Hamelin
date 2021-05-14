using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main Author: Tobias Martinsson
[CreateAssetMenu()]
public class AiBirdChasePlayer : State
{
    SomeAgent Agent;
    public float Speed;
    public float AttackDistance;
    public float attackCooldown;
    private float originalTime;
    public GameObject myPrefab;

    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    }

    public override void Enter()
    {
        Agent.NavAgent.speed = Speed;
        originalTime = attackCooldown;
    }

    public override void RunUpdate()
    {
        Agent.NavAgent.SetDestination(Agent.PlayerPosition);
        
        /*
        if (timeLeft < 0 && Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) < AttackDistance)
        {
            Destroy(Instantiate(myPrefab, new Vector3(Agent.transform.position.x, Agent.transform.position.y, Agent.transform.position.z), Quaternion.identity), 5f);

            timeLeft = originalTime;

        }
        */

        if (Physics.Linecast(Agent.transform.position, Agent.PlayerPosition, Agent.CollisionLayer))
        {
            StateMachine.ChangeState<AiBirdPatrolState>();
        }

        attackCooldown -= Time.deltaTime;
        if (attackCooldown < 0 && Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) < AttackDistance)
        {
            attackCooldown = originalTime;
            StateMachine.ChangeState<AiBirdAttack>();

        }
        

    }
}
