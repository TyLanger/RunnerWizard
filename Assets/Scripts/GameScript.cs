using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public float startRoomRadius = 6.5f;
    public Vector3 startRoomPos;

    public float endRoomRadius = 2.5f;
    public Vector3 endRoomPos;

    public MapGrid map;

    public RunnerBrain runner;
    public RangerBrain ranger;

    public GunPickup gunPickupPrefab;

    public Rule[] rules;

    CameraFollow cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraFollow>();

        StartCoroutine(DigStartingRooms());
        Invoke("StartRunner", 3);

        runner.OnRoomCreated += RoomCreated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DigStartingRooms()
    {
        yield return null;
        map.MoveCircle(startRoomPos, startRoomRadius, false);
        map.MoveCircle(endRoomPos, endRoomRadius, false);
        runner.SetupStartRoom(startRoomPos, startRoomRadius);
    }

    void StartRunner()
    {
        runner.stateMachine.SetState(new DigTunnel(runner));
    }

    void RoomCreated(Vector3 center, float radius, int roomNumber)
    {
        if (roomNumber < rules.Length)
        {
            if (rules[roomNumber] != null)
            {
                runner.CreateRule(rules[roomNumber]);
            }
        }

        switch(roomNumber)
        {
            case 1:
                runner.SpawnMinions(3);
                break;
            case 2:
                //runner.CreateRule(runner.dropRule);
                runner.SpawnMinions(4);
                break;

            case 3:
                runner.SpawnMinions(4);
                SpawnGunsAroundRoom(center, radius);
                break;
            case 4:
                Rule[] minionRuleRoom4 = { null, rules[2], null };
                runner.SpawnMinions(3, minionRuleRoom4);
                runner.SpawnRangedMinions(2);
                break;
            case 5:
                Rule[] minionRules = { rules[5], null, rules[2], null, rules[5] };

                runner.SpawnMinions(5, minionRules);
                break;
            case 6:
                Rule[] minionRulesRoom6 = { rules[6], rules[3], rules[6] };

                runner.SpawnMinions(3, minionRulesRoom6);
                runner.SpawnRangedMinions(3);
                SpawnGunsAroundRoom(center, radius);
                // room larger for next room
                runner.roomRadius += 2;
                break;
            case 7:
                StartCoroutine(PeriodicScreenShake());
                // if I don't spawn at least 1 enemy, the runner won't swap to digTunnel automatically
                runner.stateMachine.SetState(new DigTunnel(runner));
                break;
            case 8:
                break;

        }
    }

    void SpawnGunsAroundRoom(Vector3 center, float radius)
    {
        int gunsToSpawn = 4;
        // spawn near the edge. 0.9x the way from the center to the edge
        radius *= map.spacing * 0.9f;
        for (int i = 0; i < gunsToSpawn; i++)
        {
            // sin(pi) = 0
            // sin(2pi) = 0
            Vector3 spawnPoint = center + new Vector3(radius * Mathf.Sin(Mathf.PI * i * 0.5f), 0.5f, radius * Mathf.Cos(Mathf.PI * i * 0.5f));
            SpawnWeaponPickup(spawnPoint);
        }
    }

    void SpawnWeaponPickup(Vector3 position)
    {
        GunPickup p = Instantiate(gunPickupPrefab, position, transform.rotation);
        int r = Random.Range(0, p.guns.Length);
        p.SetGun(r);
    }

    IEnumerator PeriodicScreenShake()
    {
        while(true)
        {
            // vary it a little
            cam.AddShake(0.5f);
            yield return new WaitForSeconds(1);
            cam.AddShake(0.2f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
