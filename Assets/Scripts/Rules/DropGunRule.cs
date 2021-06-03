using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGunRule : Rule
{
    private void OnTriggerEnter(Collider other)
    {
        IDroppable drop = other.GetComponent<IDroppable>();
        if(drop != null)
        {
            drop.DropGun();
        }
    }

    protected override void ChainBroken()
    {
        base.ChainBroken();
        //Debug.Log("Block chain broke");
        //Destroy(tetherTarget.gameObject);
        Destroy(gameObject);
    }
}
