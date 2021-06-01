using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EscapePlayer : IState
{
    Brain myBrain;

    public EscapePlayer(Brain brain)
    {
        myBrain = brain;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        Vector3 awayFromPlayer = (myBrain.transform.position - myBrain.GetPlayerPos()).normalized;
        myBrain.SetDestination(myBrain.transform.position + awayFromPlayer * 10);

    }
}
