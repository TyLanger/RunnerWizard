using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public Gun gun;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponent<PlayerInput>();
        if(player)
        {
            player.GiveGun(gun);
            Destroy(gameObject);
        }
    }
}
