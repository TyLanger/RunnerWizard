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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DigStartingRooms());
        Invoke("StartRunner", 3);
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
}
