using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBrain : Brain
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        if(CanSeePlayer())
        {
            stateMachine.SetState(new Chase(this));
        }
        else
        {
            stateMachine.SetState(new Idle(this));
        }
    }
}
