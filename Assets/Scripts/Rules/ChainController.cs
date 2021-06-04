using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    public event System.Action OnChainBroken;

    public Transform anchorTargetA;
    public Transform anchorTargetB;

    bool hasTargetA = false;
    bool hasTargetB = false;


    Vector3 anchorPointA;
    Vector3 anchorPointB;

    public SpriteRenderer spriteRenderer;
    //public SpriteRenderer spriteRenderer2;


    // Start is called before the first frame update
    void Awake()
    {

        anchorPointA = transform.position + Vector3.forward * 0.5f;

        anchorPointB = transform.position + -Vector3.forward * 0.5f;

        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        Health h = GetComponent<Health>();
        if (h)
        {
            h.OnDeath += ChainBroken;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hasTargetA && anchorTargetA != null)
        {
            anchorPointA = anchorTargetA.position;
        }
        if (hasTargetB && anchorTargetB != null)
        {
            anchorPointB = anchorTargetB.position;
        }

        transform.position = (anchorPointA + anchorPointB) / 2;

        transform.forward = (anchorPointA - anchorPointB).normalized;
        //transform.up = Vector3.up;

        float dist = Vector3.Distance(anchorPointA, anchorPointB);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, dist);
        spriteRenderer.size = new Vector2(Mathf.Max(1, dist-2), 1);
        // second renderer just draws behind. they don;t intersect
        //spriteRenderer2.size = new Vector2(Mathf.Max(1, dist - 3), 1);

    }

    public void SetTargets(Transform targetA, Transform targetB)
    {
        anchorTargetA = targetA;
        anchorTargetB = targetB;

        hasTargetA = true;
        hasTargetB = true;
    }

    public void SetPoints(Vector3 pointA, Vector3 pointB)
    {
        anchorPointA = pointA;
        anchorPointB = pointB;
    }

    void ChainBroken(GameObject _)
    {
        //Debug.Log("Chain Broken");
        hasTargetA = false;
        hasTargetB = false;
        OnChainBroken?.Invoke();
    }
}
