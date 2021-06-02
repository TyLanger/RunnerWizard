using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{

    public float moveSpeed = 1;
    public float currentSpeed;
    Vector3 moveDir;

    public bool motorMovement = true;

    Vector3 momentumDir;
    public float lookMultiplierMultiplier = 0.2f;

    public ParticleSystem runDustParticles;
    public float dustSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        if(runDustParticles)
        {
            runDustParticles.Play();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dot = Vector3.Dot(momentumDir, moveDir);
        float lookDot = Vector3.Dot(transform.forward, moveDir);
        //Debug.DrawRay(transform.position, moveDir, Color.red);
        //Debug.DrawRay(transform.position, momentumDir*3, (dot>0.98f)?Color.green:Color.blue);

        // move faster if you continue in the same direction, but not slower when swapping direction
        float momentumMultiplier = (1 + Ease.SmoothStep(Mathf.Clamp((dot), 0, 1f)));
        // move faster if you look where you're going, but not slower if looking back
        float lookMultiplier = (1 + lookMultiplierMultiplier * Ease.SmoothStep(Mathf.Clamp(lookDot, 0, 1)));

        currentSpeed = moveSpeed * momentumMultiplier * lookMultiplier;

        if (runDustParticles != null)
        {
            if (currentSpeed > dustSpeed)
            {
                if (!runDustParticles.isPlaying)
                {
                    runDustParticles.Play();
                }
            }
            else
            {
                runDustParticles.Stop();
            }
        }

        if (motorMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDir, currentSpeed * Time.fixedDeltaTime);
        }

        momentumDir = Vector3.Lerp(momentumDir, moveDir, 0.08f);

        //momentumDir += moveDir;
        //momentumDir *= 0.5f;
        //Debug.Log($"Dot: {dot}");
    }

    public void MoveTo(Vector3 direction)
    {
        moveDir = direction;
    }

    public void SetLookAt(Vector3 worldPosition)
    {
        transform.LookAt(worldPosition);
    }
}
