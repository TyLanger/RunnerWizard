using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashAttack : IState
{
    Brain myBrain;
    public SmashAttack(Brain brain)
    {
        myBrain = brain;
    }

    public void OnEnter()
    {
        // keep moving
        //myBrain.SetDestination(myBrain.transform.position);
        if(myBrain.GetType() == typeof(ChaserBrain))
        {
            ((ChaserBrain)myBrain).StartSmashAttack();
        }
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        //myBrain.Aim(myBrain.player.position);
        myBrain.SetDestination(myBrain.player.position);

    }
}
