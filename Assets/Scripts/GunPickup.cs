using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public Gun[] guns;

    public int gunType = 0;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponent<PlayerInput>();
        if(player)
        {
            if(player.GiveGun(guns[gunType], gunType));
                Destroy(gameObject);
        }
    }

    public void SetGun(int gunType)
    {
        this.gunType = gunType;
    }
}
