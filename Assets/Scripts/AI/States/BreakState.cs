using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakState : IState
{
    RunnerBrain myBrain;
    IState stateToGoBackTo;

    Health hp;

    public BreakState(RunnerBrain brain, IState lastState)
    {
        myBrain = brain;
        stateToGoBackTo = lastState;
    }

    public void OnEnter()
    {
        // pull the player near
        // spawn a heal rule
        // kill all enemies
        // wait until fullish hp
        // go back to old state
        hp = myBrain.GetComponent<Health>();

        Vector3 meToPlayer = myBrain.player.position - myBrain.transform.position;
        Vector3 center = myBrain.transform.position + meToPlayer.normalized * 5;
        Vector3 playerSitPos = myBrain.transform.position + meToPlayer.normalized * 10;

        myBrain.StopPathing();
        myBrain.player.position = playerSitPos;
        
        PlayerCanMove(false);
        PlayerCanShoot(false);

        myBrain.CreateHealRule(center);

        // kill all enemies

    }

    public void OnExit()
    {
        PlayerCanMove(true);
        PlayerCanShoot(true);
        myBrain.StartPathing();
        // break the chain?
    }

    public void Tick()
    {
        // if(hp > 0.9f)
        if(hp.GetHealthPercent() > 0.9f)
        {
            ResumeState();
        }
        
    }

    void ResumeState()
    {
        // resume doesn't trigger enter again
        myBrain.stateMachine.ReturnToState(stateToGoBackTo);
    }

    void PlayerCanMove(bool canMove)
    {
        myBrain.player.GetComponent<PlayerInput>().CanMove(canMove);
    }

    void PlayerCanShoot(bool able)
    {
        myBrain.player.GetComponent<PlayerInput>().CanShoot(able);
    }
}

 
