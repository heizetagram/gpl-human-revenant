using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossHealth : MonoBehaviour
{
    public HealthBar healthbar;
    public int health = 500;

    public GameObject deathEffect;

    public bool isInvulnerable = false;
    
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;
        healthbar.UpdateHealthBar(200, health);
        if (health <= 200)
        {
            GetComponent<Animator>().SetBool("IsEnraged", true);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<Animator>().SetTrigger("Death");
        Destroy(gameObject);
    }

}