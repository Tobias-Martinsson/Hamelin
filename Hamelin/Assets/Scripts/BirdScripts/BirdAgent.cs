using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
//Main Author: Tobias Martinsson
public class BirdAgent : SomeAgent
{
    private StateMachine StateMachine;

    
    
    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        NavAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this, States);
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        agentTransform = GetComponent<Transform>();
        AllAgents.AddAgent(this);
    }



    private void OnDestroy()
    {
        AllAgents.removeAgent(this);
    }

    private void Update()
    {
        StateMachine.RunUpdate();
    }

}