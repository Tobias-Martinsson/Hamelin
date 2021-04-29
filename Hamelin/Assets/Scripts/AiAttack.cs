using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class AiAttack : State
{
    public float AttackDistance;
    SomeAgent Agent;
    public GameObject playerObject;
    private Transform Player; 
   

    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
    
    }

    public override void RunUpdate()
    {
        Player = playerObject.transform;
        Player.GetComponent<PlayerController3D>().setDamageDealt(true);

        
        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > AttackDistance)
        {
            Debug.Log("Chase");
            StateMachine.ChangeState<AiChasePlayer>();
        }
        
    }
}
