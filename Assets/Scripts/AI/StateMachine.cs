using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    IState currentState;

    public void Tick()
    {
        currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (currentState != null && state.GetType() == currentState.GetType())
            return;
        
        currentState?.OnExit();
        currentState = state;
        currentState?.OnEnter();
    }

    public IState GetCurrentState()
    {
        return currentState;
    }

    public void ReturnToState(IState state)
    {
        if (currentState != null && state.GetType() == currentState.GetType())
            return;

        currentState?.OnExit();
        currentState = state;
        // don't enter again
    }
    public string GetCurrentStateName()
    {
        return currentState.GetType().ToString();
    }
    
}
