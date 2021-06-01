using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

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

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
