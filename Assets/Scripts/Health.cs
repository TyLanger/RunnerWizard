using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IRuleable
{
    public event Action<GameObject> OnDeath;

    public int maxHealth = 100;
    public int currentHealth;

    bool dead = false;

    bool isProtected = false;

    bool isPlayer = false;
    CameraFollow cam;

    public GameObject healthBarPrefab;
    public Vector3 hpBarOffset;
    GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        PlayerInput p = GetComponent<PlayerInput>();
        if(p)
        {
            isPlayer = true;
            cam = FindObjectOfType<CameraFollow>();
        }
        healthBar = Instantiate(healthBarPrefab, transform.position + hpBarOffset, transform.rotation);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            healthBar.transform.position = transform.position + hpBarOffset;
        }
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
                    damage = Math.Max(1, damage / 2);
                }
            }
            if(isPlayer)
            {
                cam.AddShake(3* damage / maxHealth);
            }
            currentHealth -= damage;
            Slider s = healthBar.GetComponentInChildren<Slider>();
            s.value = Mathf.Clamp01(((float)currentHealth / maxHealth));
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        dead = true;
        Destroy(healthBar);
        OnDeath?.Invoke(gameObject);
    }

    public void Protect(bool active)
    {
        isProtected = active;   
    }
}
