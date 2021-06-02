using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    public int damage = 10;
    public bool onlyHitPlayer = true;

    private void OnTriggerEnter(Collider other)
    {
        if(onlyHitPlayer)
        {
            PlayerInput p = other.GetComponent<PlayerInput>();
            if (p == null)
                return;
        }

        Health h = other.GetComponent<Health>();
        if(h)
        {
            Debug.Log($"Hit {other.gameObject.name}");
            h.TakeDamage(10);
        }
    }
}
