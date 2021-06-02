using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static float moveSpeed = 10;
    Vector3 moveTarget;
    float scale =1;

    Vector3 startPos;

    // Start is called before the first frame update
    void Awake()
    {
        moveTarget = transform.position;
        startPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.fixedDeltaTime);
    }

    public void InitBlock(float scale)
    {
        this.scale = scale;
    }

    public void MoveUp()
    {
        moveTarget = startPos + new Vector3(0, scale, 0);
    }

    public void MoveDown()
    {
        moveTarget = startPos;
    }
}
