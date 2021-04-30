using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class AiBirdAttack : State
{
    public float AttackDistance;
    SomeAgent Agent;
    private Transform Player;
    public GameObject myPrefab;


    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
      
    }

    public override void RunUpdate()
    {
        Agent.Player.transform.GetComponent<PlayerController3D>().setDamageDealt(true);
        Destroy(Instantiate(myPrefab, new Vector3(Agent.transform.position.x, Agent.transform.position.y, Agent.transform.position.z), Quaternion.identity), 5f);
        

        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > AttackDistance)
        {
            Debug.Log("Chase");
            StateMachine.ChangeState<AiBirdChasePlayer>();
        }

    }
}
