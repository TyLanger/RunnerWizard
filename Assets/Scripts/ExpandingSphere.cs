using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingSphere : MonoBehaviour
{
    float currentRadius = 1;
    float maxRadius = 50;
    public float expansionSpeed = 5;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentRadius > maxRadius)
        {
            if (!audioSource.isPlaying)
            {
                // only destroy when the audio finishes
                // the timing is pretty close anyway
                // it just stops it from clipping and popping
                Destroy(gameObject);
            }
            return; // don't get any bigger
        }
        currentRadius += expansionSpeed * Time.deltaTime;
        transform.localScale = Vector3.one * currentRadius;
    }


    public void Setup(float maxRadius)
    {
        this.maxRadius = maxRadius;
    }
}
