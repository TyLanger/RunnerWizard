using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : IState
{
    Brain myBrain;

    public Death(Brain brain)
    {
        myBrain = brain;
    }

    public void OnEnter()
    {
        // oh dear, I've died
        myBrain.StopPathing();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}
