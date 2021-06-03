using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerBrain : Brain
{
    public float runAwayDist = 6;
    public float safeDist = 14; // feels safe enough to stop running




    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(!dead)
        {
            if(CanSeePlayer())
            {
                if(Vector3.Distance(transform.position, player.position) < runAwayDist)
                {
                    stateMachine.SetState(new EscapePlayer(this));
                }
                if (Vector3.Distance(transform.position, player.position) > safeDist)
                {
                    stateMachine.SetState(new ShootPlayer(this));
                }
            }
            else
            {
                stateMachine.SetState(new Idle(this));
            }
        }
    }
}
