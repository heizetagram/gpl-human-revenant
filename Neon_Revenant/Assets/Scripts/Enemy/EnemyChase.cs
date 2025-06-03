using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stoppingDistance = 9f;
    public float attackCooldown = 1.5f;

    public bool useJumpAttack = false; // Toggle für Sprungangriff
    public float jumpForce = 10f;
    public float jumpAttackDistance = 4f;

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
            Debug.Log(distance < jumpAttackDistance);
            if (useJumpAttack  && distance < jumpAttackDistance)
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

    void JumpTowardsPlayer(Vector2 direction)
    {
        animator.SetTrigger("Attack");
        rb.AddForce(new Vector2(direction.x * jumpForce, jumpForce), ForceMode2D.Impulse);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.TakeDamage(10);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (useJumpAttack && collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
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
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100f);
            }
        }
    }
}
