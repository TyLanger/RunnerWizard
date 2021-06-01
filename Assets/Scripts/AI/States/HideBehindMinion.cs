using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBehindMinion : IState
{
    Brain myBrain;

    public HideBehindMinion(Brain brain)
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
        if (myBrain.GetType() == typeof(RunnerBrain))
        {
            RunnerBrain b = (RunnerBrain)myBrain;
            if (b)
            {
                b.SetDestination(b.GetPointBehindMinion());
            }
        }
    }

    
}
