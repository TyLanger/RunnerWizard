using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : IState
{
    RangerBrain myBrain;

    public ShootPlayer(RangerBrain brain)
    {
        myBrain = brain;
    }
    public void OnEnter()
    {
        myBrain.SetDestination(myBrain.transform.position);
    }

    public void OnExit()
    {
        myBrain.gun.Reload();
    }

    public void Tick()
    {
        myBrain.Aim(myBrain.player.position);
        myBrain.Shoot();
        if(myBrain.gun.GetClipPercent() < 0.1f)
        {
            myBrain.gun.Reload();
        }
    }
}
