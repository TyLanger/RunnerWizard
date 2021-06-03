using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPlayer : IState
{
    Brain myBrain;
    float timeSpentWaiting;
    // go looking for the player after this?
    float maxTimeToWait = 30;

    float playerNearDistance = 15;

    public WaitForPlayer(Brain brain)
    {
        myBrain = brain;
    }

    public void OnEnter()
    {
        timeSpentWaiting = 0;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        timeSpentWaiting += Time.deltaTime;
        if(Vector3.Distance(myBrain.transform.position, myBrain.player.position) < playerNearDistance)
        {
            // player found us
            //Debug.Log($"Waited for {timeSpentWaiting} seconds");
            myBrain.stateMachine.SetState(new CreateRoom(myBrain));
        }
    }
}
