using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandBroken : IState
{
    int shotsFired = 0;
    int shotsToFire = 5;

    float timeBetweenShots = 3;
    float timeOfNextShot = 0;

    RunnerBrain myBrain;
    Vector3 roomCenter;
    float roomRadius;

    public WandBroken(RunnerBrain brain, Vector3 roomCenter, float roomRadius)
    {
        myBrain = brain;
        this.roomCenter = roomCenter;
        this.roomRadius = roomRadius;
    }

    public void OnEnter()
    {
        Debug.Log("Enter WandBroken");
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if(Time.time > timeOfNextShot)
        {
            timeOfNextShot = Time.time + timeBetweenShots;
            timeBetweenShots -= 0.25f;
            Shoot();
        }


        if(shotsFired > shotsToFire)
        {
            myBrain.stateMachine.SetState(new DigTunnel(myBrain));
        }
    }

    void Shoot()
    {
        myBrain.Shoot();
        myBrain.CastSpellEffect();

        shotsFired++;

        Vector2 randomCircle = Random.insideUnitCircle;
        Vector3 randomPoint = roomCenter + new Vector3(randomCircle.x, 0.58f, randomCircle.y) * roomRadius;
        Debug.Log($"randomCircle: {randomCircle} randomPoint: {randomPoint}");

        Vector3 randomDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
        Vector3 randomPoint2 = roomCenter + randomDir * roomRadius;
        Debug.Log($"randomDir: {randomDir} randomPoint: {randomPoint2}");

        myBrain.SetDestination(randomPoint2);
    }
}
