using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SomeAgent : MonoBehaviour
{
    public NavMeshAgent NavAgent;
    public Transform Player;
    public LayerMask CollisionLayer;
    public Vector3 point1;
    public Vector3 point2;

    public List<Transform> PatrolPoints;
    public CapsuleCollider collider;
    public State[] States; 
    public float forwardCheckDistance;
    public RaycastHit hit;

    private StateMachine StateMachine;
    public Transform GetPatrolPoint => PatrolPoints[Random.Range(0, PatrolPoints.Count)];

    public Vector3 PlayerPosition => Player.position;

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        NavAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this, States);
    }

    private void Update()
    {
        point1 = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
        point2 = transform.position + collider.center + Vector3.down * (collider.height / 2 - collider.radius);
        StateMachine.RunUpdate();


        if(Physics.CapsuleCast(point1, point2, collider.radius * 0.95f, Vector3.forward, out hit, forwardCheckDistance))
        {
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "CatchingNet")
                GetComponent<Renderer>().enabled = false;
                Destroy(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
       
    }

}
