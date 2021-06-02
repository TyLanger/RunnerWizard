using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule : MonoBehaviour
{
    public Transform tetherTarget;

    public float maxTetherDist = 3;
    public float minDist = 1;

    public float moveSpeed = 3;
    Vector3 moveTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distToTether = Vector3.Distance(transform.position, tetherTarget.position);
        if(distToTether > maxTetherDist)
        {
            moveTarget = tetherTarget.position;
        }
        else if(distToTether < minDist)
        {
            moveTarget = transform.position;
        }
        //transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, moveTarget, moveSpeed * Time.fixedDeltaTime);
    }

    
}
