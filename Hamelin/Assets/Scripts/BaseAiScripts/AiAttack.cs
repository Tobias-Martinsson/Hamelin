using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main Author: Tobias Martinsson
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
     
        Agent.Player.GetComponent<PlayerController3D>().SetDamageDealt(true);
        
        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > AttackDistance)
        {
            Debug.Log("Chase");
            StateMachine.ChangeState<AiChasePlayer>();
        }
        
    }
}
