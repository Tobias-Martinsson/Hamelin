using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BirdAgent : SomeAgent
{
    public float timeLeft;
    public float originalTime;
    public GameObject myPrefab;
    private StateMachine StateMachine;

    private void Start()
    {
        originalTime = timeLeft;
    }

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        NavAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this, States);
    }



    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(Instantiate(myPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity), 5f);
            timeLeft = originalTime;
        }

        StateMachine.RunUpdate();
    }

}