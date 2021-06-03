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
    public Gun gun;
    protected CameraFollow cam;

    public StateMachine stateMachine;
    public string CurrentStateName;
    protected bool dead = false;

    NavMeshPath path;

    protected virtual void Awake()
    {
        motor = GetComponent<Motor>();
        agent = GetComponent<NavMeshAgent>();
        cam = FindObjectOfType<CameraFollow>();

        Health h = GetComponent<Health>();
        if(h)
        {
            h.OnDeath += Die;
        }

        path = new NavMeshPath();
        agent.CalculatePath(transform.position, path);

        StateMachineSetup();
    }

    protected virtual void Update()
    {
        CurrentStateName = $"{stateMachine.GetCurrentStateName()}";
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
        if (!agent.pathPending)
        {
            SetDestination(player.position);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        agent.CalculatePath(destination, path);
        agent.SetPath(path);
        
    }

    public void StopPathing()
    {
        agent.isStopped = true;
    }

    public void Tele(Vector3 position)
    {
        agent.Warp(position);
    }

    public Vector3 GetPlayerPos()
    {
        return player.position;
    }

    public bool CanSeePlayer()
    {
        // vision blocked by blocks or the player. See through other enemies
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

    public void Aim(Vector3 worldLookPoint)
    {
        transform.LookAt(new Vector3(worldLookPoint.x, transform.position.y, worldLookPoint.z));
        //gun.transform.LookAt(new Vector3(worldLookPoint.x, gun.transform.position.y, worldLookPoint.z));
    }

    public virtual void Shoot()
    {
        gun.Fire();
    }

    void Die(GameObject go)
    {
        dead = true;
        stateMachine.SetState(new Death(this));
    }
}
