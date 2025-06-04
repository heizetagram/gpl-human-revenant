using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stoppingDistance = 9f;
    public float attackCooldown = 1.5f;

    public bool useJumpAttack = false;
    public float jumpForce = 10f;
    public float jumpAttackDistance = 4f;

    public float sightRange = 15f; 

    private float lastAttackTime;
    private SpriteRenderer sr;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= sightRange && CanSeePlayer())
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Flip Sprite
            if (direction.x != 0)
                sr.flipX = direction.x < 0;

            // Bewegung und Angriff
            if (distance > stoppingDistance)
            {
                transform.position += (Vector3)(direction * speed * Time.deltaTime);
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                if (useJumpAttack && distance < jumpAttackDistance)
                {
                    JumpTowardsPlayer(direction);
                }
                else
                {
                    Attack();
                }
                lastAttackTime = Time.time;
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector2 direction = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, sightRange);
        return hit.collider != null && hit.collider.transform == player;
    }

    void JumpTowardsPlayer(Vector2 direction)
    {
        animator.SetTrigger("Attack");
        rb.AddForce(new Vector2(direction.x * jumpForce, jumpForce), ForceMode2D.Impulse);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        PlayerHealth pc = player.GetComponent<PlayerHealth>();
        if (pc != null)
        {
            pc.TakeDamage(10);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (useJumpAttack && collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth pc = collision.gameObject.GetComponent<PlayerHealth>();
            if (pc != null)
            {
                pc.TakeDamage(10);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (transform.position.y > collision.transform.position.y + 0.5f)
            {
                rb.AddForce(Vector2.right * 100f);
            }
        }
    }
}
