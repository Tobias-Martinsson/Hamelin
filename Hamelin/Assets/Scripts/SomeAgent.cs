using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SomeAgent : MonoBehaviour
{
    public NavMeshAgent NavAgent;
    public Transform Player;
    public LayerMask CollisionLayer;

    public List<Transform> PatrolPoints;
    
    public State[] States;

    private StateMachine StateMachine;
    public Transform GetPatrolPoint => PatrolPoints[Random.Range(0, PatrolPoints.Count)];

    public Vector3 PlayerPosition => Player.position;

    private void Awake()
    {

        NavAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this, States);
    }

    private void Update()
    {
        StateMachine.RunUpdate();

    }

}
