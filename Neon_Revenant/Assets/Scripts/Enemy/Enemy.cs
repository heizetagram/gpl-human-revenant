using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int health = 100;
    public GameObject deathEffect;
    public Animator animator;

    public void Update()
    {
        float speed = Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x);

        animator.SetBool("isRunning", speed != 0);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        animator.SetTrigger("Hit");

        if (health <= 0)
        {
            animator.SetTrigger("Death");
            StartCoroutine(WaitAndDie(0.5f));
        }
    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator WaitAndDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }
}