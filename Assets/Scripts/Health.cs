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
    public bool canRespawn = false;
    Vector3 respawnPoint;

    bool isProtected = false;

    bool isPlayer = false;
    CameraFollow cam;

    public GameObject healthBarPrefab;
    public Vector3 hpBarOffset;
    GameObject healthBar;
    Slider s;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = transform.position;
        currentHealth = maxHealth;
        PlayerInput p = GetComponent<PlayerInput>();
        if(p)
        {
            isPlayer = true;
            cam = FindObjectOfType<CameraFollow>();
        }
        healthBar = Instantiate(healthBarPrefab, transform.position + hpBarOffset, transform.rotation);
        s = healthBar.GetComponentInChildren<Slider>();
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
            
            s.value = Mathf.Clamp01(((float)currentHealth / maxHealth));
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        if(canRespawn)
        {
            Respawn();
            return;
        }
        dead = true;
        Destroy(healthBar);
        OnDeath?.Invoke(gameObject);
    }

    void Respawn()
    {
        transform.position = respawnPoint;
        currentHealth = maxHealth;
        s.value = 1;
    }

    public void Protect(bool active)
    {
        isProtected = active;   
    }

    public void SetRespawnPoint(Vector3 point)
    {
        Debug.Log($"Spawn point set to {point}");
        respawnPoint = point;
    }
}
