using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule : MonoBehaviour
{
    public Transform tetherTarget;
    bool tetheredToSomething = false;
    public float maxTetherDist = 3;
    public float minDist = 1;

    public float moveSpeed = 3;
    Vector3 moveTarget;

    public float radius = 30;
    SphereCollider triggerSphere;
    RadiusIndicator indicator;


    // Start is called before the first frame update
    void Awake()
    {
        triggerSphere = GetComponent<SphereCollider>();
        triggerSphere.radius = radius;
        indicator = GetComponent<RadiusIndicator>();
        if (indicator)
        {
            indicator.xradius = radius;
            indicator.zradius = radius;
        }

        moveTarget = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tetheredToSomething)
        {
            float distToTether = Vector3.Distance(transform.position, tetherTarget.position);
            if (distToTether > maxTetherDist)
            {
                moveTarget = tetherTarget.position;
            }
            else if (distToTether < minDist)
            {
                moveTarget = transform.position;
            }
        }
        //transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, moveTarget, moveSpeed * Time.fixedDeltaTime);
    }

    public void SetChain(ChainController chain)
    {
        chain.OnChainBroken += ChainBroken;
    }

    public void SetTetherTarget(Transform target)
    {
        tetherTarget = target;
        tetheredToSomething = true;
    }

    protected virtual void ChainBroken()
    {
        tetheredToSomething = false;
    }

    protected void UpdateRadius(float newRadius)
    {
        radius = newRadius;
        triggerSphere.radius = newRadius;
        if (indicator)
        {
            indicator.xradius = newRadius;
            indicator.zradius = newRadius;
            indicator.CreatePoints();
        }
    }
    
}
