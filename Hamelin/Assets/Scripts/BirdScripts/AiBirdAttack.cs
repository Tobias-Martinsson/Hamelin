using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class AiBirdAttack : State
{
    public float AttackDistance;
    SomeAgent Agent;
    public Transform Player;
    private Scene scene;
    public GameObject myPrefab;


    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
        scene = SceneManager.GetActiveScene();
    }

    public override void RunUpdate()
    {
        SceneManager.LoadScene(scene.name);
        Destroy(Instantiate(myPrefab, new Vector3(Agent.transform.position.x, Agent.transform.position.y, Agent.transform.position.z), Quaternion.identity), 5f);
        

        if (Vector3.Distance(Agent.transform.position, Agent.PlayerPosition) > AttackDistance)
        {
            Debug.Log("Chase");
            StateMachine.ChangeState<AiBirdChasePlayer>();
        }

    }
}
