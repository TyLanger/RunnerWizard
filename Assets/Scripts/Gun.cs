using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Bullet bullet;
    int clipSize = 30;
    int currentBullets = 0;

    public float timeBetweenShots = 0.1f;
    float timeOfNextShot = 0;

    bool currentlyReloading = false;
    float reloadTime = 1;

    public Transform muzzlePoint;

    // Start is called before the first frame update
    void Start()
    {
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        if(CanFire())
        {
            timeOfNextShot = Time.time + timeBetweenShots;
            currentBullets--;
            Bullet copy = Instantiate(bullet, muzzlePoint.position, transform.rotation);
            //copy.transform.forward = transform.forward;
        }
    }

    bool CanFire()
    {
        return Time.time > timeOfNextShot && currentBullets > 0 && !currentlyReloading;
    }

    public void Reload()
    {
        if (!currentlyReloading)
        {
            currentlyReloading = true;
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        currentBullets = clipSize;
        currentlyReloading = false;
    }
}
