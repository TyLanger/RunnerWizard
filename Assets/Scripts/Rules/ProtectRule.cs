using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectRule : Rule
{

    private void OnTriggerEnter(Collider other)
    {
        IRuleable r = other.GetComponent<IRuleable>();
        if (r != null)
        {
            r.Protect(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IRuleable r = other.GetComponent<IRuleable>();
        if (r != null)
        {
            r.Protect(false);
        }
    }
}
