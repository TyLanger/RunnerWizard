using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public event System.Action<Vector3> OnBulletEnded;

    public Bullet bullet;
    public int clipSize = 30;
    protected int currentBullets = 0;

    public float timeBetweenShots = 0.1f;
    protected float timeOfNextShot = 0;

    bool currentlyReloading = false;
    float reloadTime = 1;

    public Transform muzzlePoint;

    public ParticleSystem muzzleFlashParticles;

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Fire()
    {
        if(CanFire())
        {
            timeOfNextShot = Time.time + timeBetweenShots;
            currentBullets--;
            Bullet copy = Instantiate(bullet, muzzlePoint.position, transform.rotation);
            copy.OnEnd += BulletEnded;
            if(muzzleFlashParticles)
            {
                muzzleFlashParticles.Play();
            }
            //copy.transform.forward = transform.forward;
        }
    }

    protected bool CanFire()
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

    void BulletEnded(Vector3 position)
    {
        // should I be propogating events like this?
        OnBulletEnded?.Invoke(position);
    }
}
