using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Motor))]
public class Brain : MonoBehaviour
{

    protected Motor motor;
    public Transform player;
    protected NavMeshAgent agent;

    public StateMachine stateMachine;


    protected virtual void Awake()
    {
        motor = GetComponent<Motor>();
        agent = GetComponent<NavMeshAgent>();

        StateMachineSetup();
    }

    protected virtual void Update()
    {
        stateMachine?.Tick();
    }

    protected virtual void FixedUpdate()
    {
        motor.MoveTo(agent.velocity.normalized);
        agent.speed = motor.currentSpeed;
    }

    protected virtual void StateMachineSetup()
    {
        stateMachine = new StateMachine();

        stateMachine.SetState(new Idle(this));
    }

    public void PathToPlayer()
    {
        SetDestination(player.position);
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public Vector3 GetPlayerPos()
    {
        return player.position;
    }

    public bool CanSeePlayer()
    {
        // see through other enemies
        int layerMask = ~(1 << LayerMask.NameToLayer("Enemy"));
        int playerAndBlockLayer = (1 << (LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Block")));

        if(Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hitInfo, 100, playerAndBlockLayer))
        {
            if(hitInfo.collider.transform == player)
            {
                // can see the player
                return true;
            }
        }
        return false;
    }
}
