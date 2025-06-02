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
        Vector2 direction = (player.position - transform.position).normalized;

        if (distance > stoppingDistance)
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }

        // Flip sprite correctly based on facing direction
        if (direction.x != 0)
            sr.flipX = direction.x < 0; // <- Umgedreht!
    }

    void Attack()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            pc.TakeDamage(10); 
        }
    }
}