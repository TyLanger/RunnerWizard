using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{

    public Transform[] muzzlePoints;

    public override void Fire()
    {
        if(CanFire())
        {
            timeOfNextShot = Time.time + timeBetweenShots;
            currentBullets--;
            for (int i = 0; i < muzzlePoints.Length; i++)
            {
                Instantiate(bullet, muzzlePoints[i].position, muzzlePoints[i].rotation);
            }

            //copy.OnEnd += BulletEnded;
            if (muzzleFlashParticles)
            {
                muzzleFlashParticles.Play();
            }
        }
    }
}
