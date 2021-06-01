using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerBrain : Brain
{
    // what can the runner do?
    // what does it want to do?
    // it can wave its magic wand to remove terrain
    // it wants to run away from the player
    // it creates rules which the player then tries to break
    // does it run away forever?
    // it gets tired after running of for a while

    // states?
    // idle - just wandering around waiting for the player
    // running
    // tunneling
    // zig zag
    // arena creation
    // cower
    // break time



    Vector3 spawnPos;
    float timeBetweenSwaps = 5;
    float timeOfNextMove = 0;

    float playerRadius = 7; // how far you want to be away from the player

    MapGrid map;
    public Brain chaserPrefab;
    int numMinions = 3;
    float minionSpacing = 5;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        map = FindObjectOfType<MapGrid>();

        spawnPos = transform.position;
        stateMachine.SetState(new CreateRoom(this));
    }

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(player.position, transform.position) < playerRadius && CanSeePlayer())
        {

            stateMachine.SetState(new EscapePlayer(this));

        }
        else
        {
            stateMachine.SetState(new Idle(this));
        }
    }

    public void SpawnMinions()
    {
        Debug.Log("Spawning minions");
        StartCoroutine(CreateMinions());
    }

    IEnumerator CreateMinions()
    {
        yield return null;
        yield return null;

        Vector3 midPoint = (transform.position + player.position)/2;
        Vector3 perpLine = transform.position - player.position;
        perpLine = new Vector3(-perpLine.z, perpLine.y, perpLine.x).normalized;

        for (int i = 0; i < numMinions; i++)
        {
            Brain copy = Instantiate(chaserPrefab, midPoint + perpLine * (i-(numMinions/2))*minionSpacing, Quaternion.identity);
            copy.player = player;
        }
    }

    public void SpawnRoom()
    {
        StartCoroutine(CarveRoom());
    }

    IEnumerator CarveRoom()
    {
        yield return null;
        map.MoveCircle(transform.position, 6.5f, false);

    }

}
