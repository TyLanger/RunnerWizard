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

    float playerRadius = 7; // how far you want to be away from the player

    MapGrid map;
    Vector3 recentRoomCenter;
    float recentRoomRadius;


    public Brain chaserPrefab;
    int numMinions = 3;
    float minionSpacing = 5;

    public float distBehindMinion = 15;

    List<Transform> minions;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        map = FindObjectOfType<MapGrid>();
        minions = new List<Transform>();

        spawnPos = transform.position;
        stateMachine.SetState(new CreateRoom(this));
    }

    protected override void Update()
    {
        base.Update();
        if (!dead)
        {
            if (minions.Count > 0)
            {
                stateMachine.SetState(new HideBehindMinion(this));
            }
            /*
            else
            {

                if (Vector3.Distance(player.position, transform.position) < playerRadius && CanSeePlayer())
                {

                    stateMachine.SetState(new EscapePlayer(this));

                }
                else
                {
                    stateMachine.SetState(new Idle(this));
                }
            }
            */
        }
    }

    public void SpawnMinions()
    {
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
            Health h = copy.GetComponent<Health>();
            if (h)
            {
                h.OnDeath += MinionDeath;
            }
            minions.Add(copy.transform);
            
        }
    }

    public void SpawnRoom()
    {
        StartCoroutine(CarveRoom());
    }

    IEnumerator CarveRoom()
    {
        yield return null;
        float radius = 6.5f;
        map.MoveCircle(transform.position, radius, false);
        recentRoomCenter = transform.position;
        recentRoomRadius = radius;

    }

    public Vector3 GetPointBehindMinion()
    {
        Vector3 closestPoint = transform.position;
        float closestDist = Mathf.Infinity;

        for (int i = 0; i < minions.Count; i++)
        {

            Vector3 playerToMinion = minions[i].position - player.position;
            Vector3 pointBehind = minions[i].position + playerToMinion.normalized * distBehindMinion;

            float dist = Vector3.Distance(transform.position, pointBehind);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestPoint = pointBehind;
            }

        }

        return closestPoint;
    }

    public (Vector3 position, Vector3 lookPoint) GetWallBreachPoint()
    {
        Vector3 mapCenter = map.transform.position;
        Vector3 mapCenterDir = mapCenter - recentRoomCenter;

        // radius is in blocks. Multiply by spacing to get real distance
        Vector3 position = recentRoomCenter + mapCenterDir.normalized * recentRoomRadius * map.spacing;
        Vector3 lookPoint = new Vector3(mapCenter.x, transform.position.y, mapCenter.z);

        return (position, lookPoint);
    }

    void MinionDeath(GameObject minion)
    {
        minions.Remove(minion.transform);
        if(minions.Count == 0)
        {
            // all minions dead
            Debug.Log("All minions dead");
            stateMachine.SetState(new DigTunnel(this));

        }
    }

}
