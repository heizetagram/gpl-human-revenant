using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossHealth : MonoBehaviour, IDamageable
{
    private HealthBar _healthbar;
    public int health = 500;
    
    public GameObject deathEffect;

    public bool isInvulnerable = false;
    public bool isEnraged = false;
    public void Start()
    {
        _healthbar = GetComponentInChildren<HealthBar>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;
        
        GetComponent<Animator>().SetTrigger("Hurt");
        health -= damage;
      
       if (health <= 200)
       {
           isEnraged = true;
       }

        if (health <= 0)
        {
            Die();
        }
        _healthbar.UpdateHealthBar(500, health);
    }

    void Die()
    {
        GetComponent<Animator>().SetTrigger("Death");
        StartCoroutine(DestroyAfterDelay(1f));
    }
    
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}