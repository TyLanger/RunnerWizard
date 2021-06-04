using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Vector3 offset;
    public Vector3 angle;

    public Transform player;

    public float speed;

    // screen shake
    public float trauma = 0;
    public float traumaDecay = 0.01f;
    public float maxShake = 0.4f;


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 shakeOffset = new Vector3(maxShake * Ease.Cube(trauma) * Random.Range(-1, 2), maxShake * Ease.Cube(trauma) * Random.Range(-1, 2), 0);
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.fixedDeltaTime * speed)  +shakeOffset;
        
        AddShake(-traumaDecay);
    }

    public void AddShake(float amount)
    {
        trauma += amount;
        trauma = Mathf.Clamp01(trauma);
    }
}
