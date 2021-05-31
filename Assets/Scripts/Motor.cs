using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{

    public float moveSpeed = 1;
    Vector3 moveDir;

    Vector3 momentumDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dot = Vector3.Dot(momentumDir, moveDir);
        //Debug.DrawRay(transform.position, moveDir, Color.red);
        //Debug.DrawRay(transform.position, momentumDir*3, (dot>0.98f)?Color.green:Color.blue);

        transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, moveSpeed * Time.fixedDeltaTime * (1+Ease.SmoothStep(Mathf.Clamp((dot), 0, 1f))));

        momentumDir = Vector3.Lerp(momentumDir, moveDir, 0.08f);

        //momentumDir += moveDir;
        //momentumDir *= 0.5f;
        //Debug.Log($"Dot: {dot}");
    }

    public void MoveTo(Vector3 direction)
    {
        moveDir = direction;
    }
}
