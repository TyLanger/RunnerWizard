using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IRuleable
{
    public event Action<GameObject> OnDeath;

    public int maxHealth = 100;
    public int currentHealth;

    bool dead = false;

    bool isProtected = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (!dead)
        {
            if(isProtected)
            {
                if(damage > 0)
                {
                    // don't turn 1 damage to 0
                    // also don't turn 0 damage to 1
                    damage = Math.Min(1, damage / 2);
                }
            }
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        dead = true;
        OnDeath?.Invoke(gameObject);
    }

    public void Protect(bool active)
    {
        isProtected = active;   
    }
}
