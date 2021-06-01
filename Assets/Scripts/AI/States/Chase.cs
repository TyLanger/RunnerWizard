using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    Brain myBrain;

    public Chase(Brain brain)
    {
        myBrain = brain;
    }

    public void OnEnter()
    {
        //Debug.Log("Enter Chase");
    }

    public void OnExit()
    {
        //Debug.Log("Exit Chase");

    }

    public void Tick()
    {
        //Debug.Log("Chase Tick");
        myBrain.PathToPlayer();
    }
}
