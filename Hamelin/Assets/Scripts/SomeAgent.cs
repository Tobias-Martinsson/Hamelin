using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Main Author: Tobias Martinsson
public class SomeAgent : MonoBehaviour
{
    public NavMeshAgent NavAgent;
    public Transform Player;
    public LayerMask CollisionLayer;
    public Vector3 point1;
    public Vector3 point2;

    public List<Transform> PatrolPoints;
    public new CapsuleCollider collider;
    public State[] States; 

    private StateMachine StateMachine;
    public Transform GetPatrolPoint => PatrolPoints[Random.Range(0, PatrolPoints.Count)];

    public Vector3 PlayerPosition => Player.position;

    private void Awake()
    {
        collider = GetComponent <CapsuleCollider>();
        NavAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this, States);
        //Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        StateMachine.RunUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
       
    }

}
