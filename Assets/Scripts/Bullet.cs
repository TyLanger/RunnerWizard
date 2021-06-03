using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public event Action<Vector3> OnEnd;

    public float moveSpeed = 10;
    public int damage = 1;
    public bool onlyHitPlayer = false;

    public float lifetime = 10;

    public ParticleSystem impactParticles;
    bool inCleanup = false;

    private void Start()
    {
        //impactParticles = GetComponentInChildren<ParticleSystem>();
        Invoke("DestroyThis", lifetime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.fixedDeltaTime);
    }

    protected void DestroyThis()
    {
        if (!inCleanup)
        {
            OnEnd?.Invoke(transform.position);
            inCleanup = true;
            // doing this so the particles have a chance to play
            moveSpeed = 0;
            Invoke("Cleanup", 0.15f);
        }
    }

    void Cleanup()
    {
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!inCleanup)
        {
            if (onlyHitPlayer)
            {
                PlayerInput p = other.GetComponent<PlayerInput>();
                if (p == null)
                    return;
            }


            Health h = other.gameObject.GetComponent<Health>();
            if (h != null)
            {
                if (impactParticles)
                {
                    impactParticles.Play();
                }
                h.TakeDamage(damage);
                FindObjectOfType<CameraFollow>().AddShake(0.15f);
                DestroyThis();
            }
        }
    }
}
