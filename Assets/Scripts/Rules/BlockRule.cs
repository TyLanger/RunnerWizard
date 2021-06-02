using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRule : Rule
{
    protected override void ChainBroken()
    {
        base.ChainBroken();
        //Debug.Log("Block chain broke");
        Destroy(tetherTarget.gameObject);
        Destroy(gameObject);
    }
}
