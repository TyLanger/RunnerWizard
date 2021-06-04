using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public event System.Action OnGunPickedUp;

    public Gun[] guns;
    public GameObject[] gunIcons;

    public int gunType = 0;

    void Start()
    {
        UpdateGunIcons();
    }

    private void Update()
    {
        transform.RotateAround(Vector3.up, Time.deltaTime * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponent<PlayerInput>();
        if(player)
        {
            OnGunPickedUp?.Invoke();
            if(player.GiveGun(guns[gunType], gunType))
                Destroy(gameObject);
        }
    }

    public void SetGun(int gunType)
    {
        this.gunType = gunType;
        UpdateGunIcons();
    }

    void UpdateGunIcons()
    {
        gunIcons[0].SetActive(false);
        gunIcons[1].SetActive(false);

        gunIcons[gunType].SetActive(true);
    }
}
