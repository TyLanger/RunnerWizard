using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

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

    public event Action<Vector3, float, int> OnRoomCreated;

    Vector3 spawnPos;

    float playerRadius = 7; // how far you want to be away from the player

    MapGrid map;
    Vector3 recentRoomCenter;
    float recentRoomRadius;


    public Brain chaserPrefab;
    int numMinions = 2;
    float minionSpacing = 5;
    public Brain rangerPrefab;

    public float distBehindMinion = 15;

    List<Transform> minions;

    int quadsVisited = 0;

    public Rule ruleToSpawn;
    public ChainController chain;
    public Rule blockerRule;
    public Rule dropRule;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        map = FindObjectOfType<MapGrid>();
        minions = new List<Transform>();

        spawnPos = transform.position;
        //stateMachine.SetState(new CreateRoom(this));

        //CreateRule(ruleToSpawn);
        //CreateRule(dropRule);

    }

    protected override void Update()
    {
        base.Update();
        if (!dead)
        {

        }
    }

    public void CreateRule(Rule rule)
    {
        // pick a rule
        // spawn it behind me
        Rule copy = Instantiate(rule, transform.position + Vector3.back * 2f, transform.rotation);
        ChainController chainCopy = Instantiate(chain, transform.position + Vector3.back, transform.rotation);

        chainCopy.SetTargets(transform, copy.transform);
        copy.SetChain(chainCopy);
        copy.SetTetherTarget(transform);
    }

    public void CreateRule(Rule rule, Vector3 anchorA, Vector3 anchorB)
    {
        Rule copy = Instantiate(rule, transform.position + Vector3.back * 2f, transform.rotation);
        ChainController chainCopy = Instantiate(chain, transform.position + Vector3.back, transform.rotation);

        chainCopy.SetPoints(anchorA, anchorB);
        copy.SetChain(chainCopy);
        copy.SetTetherTarget(chainCopy.transform); // will this cause them to move around?
    }

    public void CreateBlockRule(Vector3 spawnPoint, Vector3 forward)
    {
        Rule r = Instantiate(blockerRule, spawnPoint, transform.rotation);
        ChainController chainCopy = Instantiate(chain, spawnPoint, transform.rotation);

        r.transform.localScale = Vector3.one * 20;
        r.transform.forward = forward;

        chainCopy.SetPoints(r.transform.position + 3*r.transform.right, r.transform.position + -3*r.transform.right);
        r.SetChain(chainCopy);
        r.SetTetherTarget(chainCopy.transform);
        chainCopy.GetComponent<Health>().maxHealth = 10;
    }

    public void SpawnMinions()
    {
        StartCoroutine(CreateMinions());
    }

    IEnumerator CreateMinions()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 midPoint = (transform.position + player.position)/2;
        Vector3 perpLine = transform.position - player.position;
        perpLine = new Vector3(-perpLine.z, perpLine.y, perpLine.x).normalized;

        int minionsToSpawn = numMinions + quadsVisited;

        for (int i = 0; i < minionsToSpawn; i++)
        {
            Brain copy = Instantiate(chaserPrefab, midPoint + perpLine * (i-(minionsToSpawn/2))*minionSpacing, Quaternion.identity);
            copy.player = player;
            Health h = copy.GetComponent<Health>();
            if (h)
            {
                h.OnDeath += MinionDeath;
            }
            minions.Add(copy.transform);
            
        }

        stateMachine.SetState(new HideBehindMinion(this));
    }

    public void SpawnRangedMinions(int num)
    {
        Vector3 playerToMeDir = transform.position - player.position;
        Vector3 perpLine = new Vector3(-playerToMeDir.z, transform.position.y, playerToMeDir.x).normalized;
        Vector3 behindPoint = transform.position + playerToMeDir.normalized * 10;

        for (int i = 0; i < num; i++)
        {
            Brain copy = Instantiate(rangerPrefab, behindPoint + perpLine * (i - (num / 2)) * minionSpacing, Quaternion.identity);
            copy.player = player;
            Health h = copy.GetComponent<Health>();
            if(h)
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
        float radius = 5.5f;
        map.MoveCircle(transform.position, radius, false);
        recentRoomCenter = transform.position;
        recentRoomRadius = radius;

        OnRoomCreated?.Invoke(recentRoomCenter, recentRoomRadius, quadsVisited);
    }

    public void SetupStartRoom(Vector3 center, float radius)
    {
        recentRoomCenter = center;
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
        Vector3 position = recentRoomCenter + mapCenterDir.normalized * recentRoomRadius * map.spacing * 0.7f;
        Vector3 lookPoint = new Vector3(mapCenter.x, transform.position.y, mapCenter.z);

        // doesn't work for visited 1,3,5

        switch (quadsVisited)
        {
            case 1:
                lookPoint += Vector3.left * 60;
                break;
            case 3:
                lookPoint += Vector3.forward * 60;
                break;
            case 5:
                lookPoint += Vector3.right * 60;
                break;
        }

        Debug.DrawLine(transform.position, lookPoint, Color.magenta, 30);

        return (position, lookPoint);
    }

    public Vector3 GetNewQuadrant()
    {

        // just going in order now
        quadsVisited++;


        Vector3 centerOfNewQuad = map.GetQuadrantCenter(quadsVisited);
        return centerOfNewQuad;
    }

    public override void Shoot()
    {
        base.Shoot();
        //gun.OnBulletEnded += BulletEnded;
    }

    void BulletEnded(Vector3 position)
    {
        gun.OnBulletEnded -= BulletEnded;
        SetDestination(position);
        stateMachine.SetState(new FollowTunnel(this));
    }

    void MinionDeath(GameObject minion)
    {
        minions.Remove(minion.transform);
        if(minions.Count < 2)
        {
            // all minions dead
            //Debug.Log("All minions dead");
            stateMachine.SetState(new DigTunnel(this));

        }
    }

}
