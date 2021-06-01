using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom : IState
{
    Brain myBrain;

    public CreateRoom(Brain brain)
    {
        myBrain = brain;
    }

    public void OnEnter()
    {
        if (myBrain.GetType() == typeof(RunnerBrain))
        {
            RunnerBrain b = (RunnerBrain)myBrain;
            if (b)
            {
                b.SpawnRoom();
                b.SpawnMinions();
            }
        }
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        
    }
}
