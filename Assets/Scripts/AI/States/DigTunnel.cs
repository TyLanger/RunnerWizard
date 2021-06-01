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
                (destination, lookPoint) = b.GetWallBreachPoint();
                destFound = true; // so not using null vector in Tick. Is OnEnter guarenteed to run before tick?
                b.SetDestination(destination);
            }
        }
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if (hasShot)
        {
            myBrain.SetDestination(myBrain.transform.forward * 2);
            // how do I tell when the bullet reached the end?
            // should I shoot again? Or make a room?
        }
        else
        {
            if (destFound)
            {
                if (Vector3.Distance(destination, myBrain.transform.position) < shootingRange)
                {


                    myBrain.Aim(lookPoint);
                    hasShot = true;
                    myBrain.Shoot();

                    // follow the bullet into the tunnel
                    // setDest forward + a bit?
                    // setDest bullet?

                }
            }
        }
    }
}
