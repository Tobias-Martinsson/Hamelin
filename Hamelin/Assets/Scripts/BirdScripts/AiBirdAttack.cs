using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class AiBirdAttack : State
{
    public float AttackDistance;
    SomeAgent Agent;
    public GameObject myPrefab;
    public float timeLeft;
    private float originalTime;
    private int counter = 0;
    public int maxBombs;


    protected override void Initialize()
    {
        Agent = (SomeAgent)Owner;
        Debug.Assert(Agent);
        originalTime = timeLeft;
      
    }

    public override void Enter()
    {
        timeLeft = originalTime;
    }

    public override void RunUpdate()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(Instantiate(myPrefab, new Vector3(Agent.transform.position.x, Agent.transform.position.y, Agent.transform.position.z), Quaternion.identity), 2.5f);
            counter++;
            timeLeft = originalTime;
            if(counter == maxBombs)
            {
                counter = 0;
                StateMachine.ChangeState<AiBirdChasePlayer>();
            }
        }

    }
}
