using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Main Author: Tobias Martinsson
public class BirdAgent : SomeAgent
{
    private StateMachine StateMachine;

    
    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        NavAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this, States);

        agentTransform = GetComponent<Transform>();

        AllAgents.AddAgent(this);

    }

    private void Update()
    {
        StateMachine.RunUpdate();
    }

}