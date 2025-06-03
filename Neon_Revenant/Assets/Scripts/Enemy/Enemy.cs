using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int health = 100;
    public GameObject deathEffect;
    private Animator _animator;
    public GameObject[] barcodeLootPrefabs; 
    public int lootDropCount = 1;  
    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        float speed = Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        _animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            _animator.SetTrigger("Death");
            StartCoroutine(WaitAndDie(1f));
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        DropLoot();
        Destroy(gameObject);
    }
    private void DropLoot()
    {
        for (int i = 0; i < lootDropCount; i++)
        {
            if (barcodeLootPrefabs.Length == 0) return;

            GameObject selectedLoot = barcodeLootPrefabs[UnityEngine.Random.Range(0, barcodeLootPrefabs.Length)];
            
            Vector3 spawnPosition = transform.position + new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0.5f, 0);
            Instantiate(selectedLoot, spawnPosition, Quaternion.identity);
        }
    }
    private IEnumerator WaitAndDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }
}