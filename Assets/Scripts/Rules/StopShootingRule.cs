using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopShootingRule : Rule
{
    private void OnTriggerEnter(Collider other)
    {
        ICantShoot shooter = other.GetComponent<ICantShoot>();
        if (shooter != null)
        {
            shooter.CanShoot(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ICantShoot shooter = other.GetComponent<ICantShoot>();
        if (shooter != null)
        {
            shooter.CanShoot(true);
        }
    }

    protected override void ChainBroken()
    {
        base.ChainBroken();
        UpdateRadius(radius / 3);
    }
}
