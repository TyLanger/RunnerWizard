using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerBullet : Bullet
{
    MapGrid grid;
    public float digRadius = 0.75f;
    public float travelDist;
    Vector3 spawnPos;

    private void Awake()
    {
        grid = FindObjectOfType<MapGrid>();
        spawnPos = transform.position;
    }

    private void Update()
    {
        if(Vector3.Distance(spawnPos, transform.position) > travelDist)
        {
            grid.MoveCircle(transform.position, digRadius, false); // one last smash in case you're close to a wall
            //Debug.Log("Went the distance");
            DestroyThis();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Block>() != null)
        {
            grid.MoveCircle(transform.position, digRadius, false);
        }
    }
}
