using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour, IDamageable
{
    public GameObject gewinnUI;
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
        
        if (gewinnUI != null)
            gewinnUI.SetActive(true);
        
        StartCoroutine(ZuruckZumMenu());
    }
    
    private IEnumerator ZuruckZumMenu()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
        Debug.Log("fertig");
    }

}