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

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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

            // 5(blockSize)/2 = 2.5
            // 2/5 /0.2(fixedDeltaTime) = 12.5
            // when going faster than 12.5, you can move to the far side of the block
            // and then physics pushes you out that way I think
            if (currentSpeed > 12)
            {
                int blockLayer = (1 << LayerMask.NameToLayer("Block") | 1 << LayerMask.NameToLayer("BulletPassThrough"));
                // check if a wall is nearby. Don't go through it
                if (Physics.Raycast(transform.position, transform.position + moveDir, out RaycastHit hit, currentSpeed * Time.fixedDeltaTime, blockLayer))
                {
                    if (hit.distance < currentSpeed * Time.fixedDeltaTime)
                    {
                        currentSpeed = (hit.distance - 0.1f) / Time.fixedDeltaTime;
                    }
                }
            }

            rb.MovePosition(rb.position + (moveDir *currentSpeed * Time.fixedDeltaTime));
        }

        momentumDir = Vector3.Lerp(momentumDir, moveDir, 0.08f);
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
