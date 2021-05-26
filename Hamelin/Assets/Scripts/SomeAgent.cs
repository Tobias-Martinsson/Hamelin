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

    public Transform agentTransform;
    private Vector3 agentSavePosition;
    private Quaternion agentSaveRotation;

    public List<Transform> PatrolPoints;
    public new CapsuleCollider collider;
    public State[] States;

    private StateMachine StateMachine;
    public Transform GetPatrolPoint => PatrolPoints[Random.Range(0, PatrolPoints.Count)];

    public Vector3 PlayerPosition => Player.position;

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        NavAgent = GetComponent<NavMeshAgent>();
        StateMachine = new StateMachine(this, States);

        agentTransform = GetComponent<Transform>();

        AllAgents.AddAgent(this);

        Player = GameObject.FindGameObjectWithTag("Player").transform;


    }

    private void Update()
    {
        StateMachine.RunUpdate();
    }

    public void SetPatrolPoints(List<Transform> pPoints)
    {
        PatrolPoints = pPoints;
    }

    private void OnDestroy()
    {
        Debug.Log("Removed rat from list");
        AllAgents.removeAgent(this);
        //AllAgents.allAgentSaveData.Remove(this);
    }
    public void AgentSaveTransform() {
        
        agentSavePosition = agentTransform.position;
        agentSaveRotation = agentTransform.rotation;
        Debug.Log(agentSavePosition+ " IS SAVED" );
    }
   
    public void AgentResetTransform() {

        NavAgent.Warp(agentSavePosition);
        NavAgent.transform.rotation = agentSaveRotation;
        Debug.Log(NavAgent.transform.position + ""+  agentSavePosition + " SHOULD BE RESET");
    }

}
