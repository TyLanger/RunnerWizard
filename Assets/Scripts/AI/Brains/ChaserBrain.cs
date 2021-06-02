using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBrain : Brain
{

    public float attackRange = 3;
    public float smashMoveSpeed = 6.5f;
    public float restSpeed = 3.5f;
    public GameObject weapon;
    public Vector3 weaponRestPos;
    public Vector3 weaponWindupPos;
    public Vector3 weaponSmashPos;
    public float windupTime = 1;
    public float smashTime = 1;
    public float smashHoldTime = 1;
    public float backToRestTime = 1;
    bool smashing = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        if (!dead && !smashing)
        {
            if (CanSeePlayer())
            {
                if (Vector3.Distance(transform.position, player.position) < attackRange)
                {
                    stateMachine.SetState(new SmashAttack(this));
                }
                else
                {
                    stateMachine.SetState(new Chase(this));
                }
            }
            else
            {
                stateMachine.SetState(new Idle(this));
            }
        }
    }

    public void StartSmashAttack()
    {
        StartCoroutine(Smash());
    }

    IEnumerator Smash()
    {
        smashing = true;
        float startBaseSpeed = motor.moveSpeed;
        motor.moveSpeed = smashMoveSpeed;
        float startTime = Time.time;
        Vector3 weaponStartPos = weapon.transform.localPosition;
        while(Time.time < (startTime+windupTime))
        {
            //Aim(player.position); // at this stage, can still track
            float t = (Time.time - startTime) / windupTime;
            weapon.transform.localPosition = Vector3.Lerp(weaponStartPos, weaponStartPos + weaponWindupPos, t);
            yield return null;
        }

        weapon.GetComponent<Collider>().enabled = true;
        startTime = Time.time;
        while(Time.time < (startTime + smashTime))
        {
            //Aim(player.position);
            float t = (Time.time - startTime) / smashTime;
            weapon.transform.localPosition = Vector3.Lerp(weaponStartPos + weaponWindupPos, weaponStartPos + weaponSmashPos, t);
            yield return null;
        }

        motor.moveSpeed = restSpeed;
        FindObjectOfType<CameraFollow>().AddShake(0.3f);
        yield return new WaitForSeconds(smashHoldTime);
        weapon.GetComponent<Collider>().enabled = false;


        startTime = Time.time;
        while (Time.time < (startTime + backToRestTime))
        {
            float t = (Time.time - startTime) / backToRestTime;
            weapon.transform.localPosition = Vector3.Lerp(weaponStartPos + weaponSmashPos, weaponStartPos + weaponRestPos, t);
            yield return null;
        }

        motor.moveSpeed = startBaseSpeed;
        smashing = false;

        stateMachine.SetState(new Idle(this));
    }
}
