using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingSphere : MonoBehaviour
{
    float currentRadius = 1;
    float maxRadius = 10;
    public float expansionSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        currentRadius += expansionSpeed * Time.deltaTime;
        transform.localScale = Vector3.one * currentRadius;
        if(currentRadius > maxRadius)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(float maxRadius)
    {
        this.maxRadius = maxRadius;
    }
}
