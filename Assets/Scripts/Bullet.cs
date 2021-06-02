using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public event Action<Vector3> OnEnd;

    public float moveSpeed = 10;
    public int damage = 1;

    public float lifetime = 10;

    private void Start()
    {
        Invoke("DestroyThis", lifetime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.fixedDeltaTime);
    }

    protected void DestroyThis()
    {
        OnEnd?.Invoke(transform.position);
        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Health h = other.gameObject.GetComponent<Health>();
        if (h != null)
        {
            h.TakeDamage(damage);
            DestroyThis();
        }
    }
}
