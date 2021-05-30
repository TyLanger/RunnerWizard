using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{

    public float moveSpeed = 1;
    Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, moveSpeed * Time.fixedDeltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        moveDir = direction;
    }
}
