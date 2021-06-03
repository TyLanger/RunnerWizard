using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealRule : Rule
{
    private void OnTriggerStay(Collider other)
    {
        IHealable h = other.GetComponent<IHealable>();
        if(h != null)
        {
            
            h.Heal(2);
        }
    }

    protected override void ChainBroken()
    {
        base.ChainBroken();
        Destroy(gameObject);
    }
}
