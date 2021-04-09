using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class AiAttack : State
{
    public float AttackDistance;
    SomeAgent Agent;
    public Transform Player;
    private Scene scene;


    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
        scene = SceneManager.GetActiveScene();
    }

    public override void RunUpdate()
    {
        SceneManager.LoadScene(scene.name);

        
        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) < AttackDistance)
        {
            Debug.Log("Chase");
            StateMachine.ChangeState<AiChasePlayer>();
        }
        
    }
}
