using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTunnel : IState
{
    Brain myBrain;
    Vector3 destination;
    Vector3 lookPoint;
    float shootingRange = 1;
    bool hasShot = false;
    bool destFound = false;

    Vector3 centerOfNewQuad;
    float proximityToCenter = 2;

    Vector3 breachPoint;
    //int numShots = 0;
    float bulletRange = 0;

    float stuckTimer = 0;
    float timeToTele = 7;
    bool reachedBreachPoint = false;

    bool withinRangeOfCenter = false;

    public DigTunnel(Brain brain)
    {
        myBrain = brain;
    }

    public void OnEnter()
    {
        // move towards a nearby wall
        if (myBrain.GetType() == typeof(RunnerBrain))
        {
            RunnerBrain b = (RunnerBrain)myBrain;
            if (b)
            {
                b.UpdatePlayerSpawn();

                (breachPoint, lookPoint) = b.GetWallBreachPoint();
                destination = breachPoint;
                destFound = true; // so not using null vector in Tick. Is OnEnter guarenteed to run before tick?
                myBrain.SetDestination(destination);
                //b.SetDestination(destination);
                //Debug.Log($"Move to {destination}");
                Debug.DrawLine(myBrain.transform.position, destination, Color.red, 30);

                // create a block so the player can't follow so quickly
                Vector3 toCenterDir = (lookPoint - breachPoint).normalized;
                // *2 put the block a bit into where the tunnel will be
                b.CreateBlockRule(breachPoint + toCenterDir * 5, toCenterDir);

                //myBrain.gun.OnBulletEnded += BulletEnded;
                Bullet bullet = myBrain.gun.bullet;
                if(bullet.GetType() == typeof(DiggerBullet))
                {
                    bulletRange = ((DiggerBullet)bullet).travelDist;
                }

                centerOfNewQuad = b.GetNewQuadrant();
            }
        }
    }

    public void OnExit()
    {
        // moved out of OnExit to work with BreakState
        // leaving this state to go to BreakState while a bullet is in the air would break stuff
        // now you enter breakstate, then the bullet reaches and BulletEnded runs
        // when you get back, should have the update info
        //myBrain.gun.OnBulletEnded -= BulletEnded;

    }

    public void Tick()
    {
        if(!reachedBreachPoint && timeToTele < stuckTimer)
        {
            // tele if stuck before getting to the breach point
            myBrain.Tele(destination);
            Debug.Log("Had to tele");
        }
        stuckTimer += Time.deltaTime;
        //Debug.Log($"Has shot: {hasShot} Dest found: {destFound}");
        if (!hasShot)
        {
            if (destFound)
            {
                myBrain.SetDestination(destination);

                if (Vector3.Distance(destination, myBrain.transform.position) < shootingRange)
                {
                    myBrain.Aim(lookPoint);
                    hasShot = true;
                    reachedBreachPoint = true;
                    myBrain.gun.OnBulletEnded += BulletEnded;
                    myBrain.Shoot();
                    myBrain.SetDestination(myBrain.transform.position);
                }
            }
        }

        if(Vector3.Distance(myBrain.transform.position, centerOfNewQuad) < proximityToCenter)
        {
            //Debug.Log("Made it to a new quad");
            myBrain.SetDestination(myBrain.transform.position);
            myBrain.stateMachine.SetState(new WaitForPlayer(myBrain));
        }
    }

    void BulletEnded(Vector3 position)
    {
        myBrain.gun.OnBulletEnded -= BulletEnded;

        //Debug.Log("Bullet Ended");
        Debug.DrawLine(myBrain.transform.position, position, Color.blue, 20);
        //myBrain.SetDestination(position);
        hasShot = false; // get ready for a second shot
        destination = position;
        // moving handled in tick
        //myBrain.SetDestination(destination);
        //lookPoint = GetAimPoint();// new Vector3(centerOfNewQuad.x, myBrain.transform.position.y, centerOfNewQuad.z);
        lookPoint = GetAimPointForZigZag();
    }

    Vector3 GetAimPointForZigZag()
    {
        Debug.DrawLine(breachPoint, centerOfNewQuad, Color.black, 60);

        if (Vector3.Distance(destination, centerOfNewQuad) < bulletRange)
        {
            //Debug.Log("Aim right at the center");
            if(withinRangeOfCenter)
            {
                // if you get here again, you're stuck and going back and forth
                StuckAtEnd();
                return centerOfNewQuad;
            }
            withinRangeOfCenter = true;
            return centerOfNewQuad;
        }

        Vector3 breachToQuadDir = centerOfNewQuad - breachPoint;
        int desiredZigs = 5;

        // larger movement in horizontal direction
        bool horizontal = Mathf.Abs(breachToQuadDir.x) > Mathf.Abs(breachToQuadDir.z);
        float stepDistance = Mathf.Max(Mathf.Abs(breachToQuadDir.x), Mathf.Abs(breachToQuadDir.z)) / desiredZigs;
        float zigDist = Mathf.Sqrt(bulletRange * bulletRange - stepDistance * stepDistance);

        //Debug.Log($"Horizontal: {horizontal}");
        Vector3 aimPoint = Vector3.zero;
        if(horizontal)
        {
            float stepSign = Mathf.Sign(centerOfNewQuad.x - destination.x);
            float zigSign = Mathf.Sign(centerOfNewQuad.z - destination.z);

            // uses current dest so don't need to account for which shot this is
            aimPoint = destination + new Vector3(stepDistance * stepSign, 0, zigDist * zigSign);
        }
        else
        {
            float stepSign = Mathf.Sign(centerOfNewQuad.z - destination.z);
            float zigSign = Mathf.Sign(centerOfNewQuad.x - destination.x);

            // uses current dest so don't need to account for which shot this is
            aimPoint = destination + new Vector3(zigDist * zigSign, 0, stepDistance * stepSign);
        }

        return aimPoint;
    }

    void StuckAtEnd()
    {
        // 0.58 is the height of the runner
        Debug.Log("Runner was stuck");
        myBrain.Tele(new Vector3(centerOfNewQuad.x, 0.58f, centerOfNewQuad.z));
        myBrain.stateMachine.SetState(new WaitForPlayer(myBrain));

    }
}
