using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    Brain myBrain;
    public Idle(Brain brain)
    {
        myBrain = brain;
    }
    public void OnEnter()
    {
        //Debug.Log($"Entered Idle");
    }

    public void OnExit()
    {
        //Debug.Log($"Exited Idle");

    }

    public void Tick()
    {
        //Debug.Log($"Tick Idle");
    }
}
