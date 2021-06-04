using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealWand : IState
{
    public event System.Action OnWandStolen;

    RunnerBrain myBrain;

    float wandProximity = 6;
    Vector3 wandPos;

    public StealWand(RunnerBrain brain, Vector3 wandPosition)
    {
        myBrain = brain;
        wandPos = wandPosition;
    }

    public void OnEnter()
    {
        myBrain.SetDestination(wandPos);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if(Vector3.Distance(wandPos, myBrain.transform.position) < wandProximity)
        {
            // done
            OnWandStolen?.Invoke();
            myBrain.stateMachine.SetState(new CreateRoom(myBrain));
        }
    }
}
