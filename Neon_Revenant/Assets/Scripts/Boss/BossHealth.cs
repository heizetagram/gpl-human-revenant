using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossHealth : MonoBehaviour, IDamageable
{
    public HealthBar healthbar;
    public int health = 500;

    public GameObject deathEffect;

    public bool isInvulnerable = false;
    
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;
        
        GetComponent<Animator>().SetTrigger("Hurt");
        health -= damage;
      
       // if (health <= 200)
        //{
        //    GetComponent<Animator>().SetBool("IsEnraged", true);
        //}

        if (health <= 0)
        {
            Die();
        }
        
        //healthbar.UpdateHealthBar(200, health);
    }

    void Die()
    {
        GetComponent<Animator>().SetTrigger("Death");
        StartCoroutine(DestroyAfterDelay(1.5f));
    }
    
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}