using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stoppingDistance = 9f;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
   
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
   
        
        if (distance > stoppingDistance)
        {
            
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
            
            if (direction.x != 0)
                sr.flipX = direction.x > 0;
        } else if (Time.time >= lastAttackTime + attackCooldown)
        {
                Attack();
                lastAttackTime = Time.time;
        }
    }
    void Attack()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.TakeDamage(10); 
        }
    }
}