using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stoppingDistance = 9f;
    public float attackCooldown = 1.5f;
    public float lookAroundInterval = 2f; 
    private float _nextLookTime = 0f;
    public Transform groundCheck;
    public float groundCheckDistance = 5f;
    public LayerMask groundLayer;
    public bool useJumpAttack = false;
    public float jumpForce = 10f;
    public float jumpAttackDistance = 4f;

    public float sightRange = 15f; 

    private float _lastAttackTime;
    private SpriteRenderer _sr;
    private Animator _animator;
    private Rigidbody2D _rb;

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;
        if (Time.time >= _nextLookTime)
        {
            LookAtPlayer();
            _nextLookTime = Time.time + lookAroundInterval;
        }
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= sightRange )
        {
            Vector2 direction = (player.position - transform.position).normalized;

            if (direction.x != 0)
                _sr.flipX = direction.x < 0;
            if (distance > stoppingDistance && IsGroundAhead())
                transform.position += (Vector3)(direction * (speed * Time.deltaTime));
            else if (Time.time >= _lastAttackTime + attackCooldown && distance <= stoppingDistance )
            {
                if (useJumpAttack && distance < jumpAttackDistance)
                    JumpTowardsPlayer(direction);
                else
                    Attack();
                _lastAttackTime = Time.time;
            }
        }
    }
    
    void JumpTowardsPlayer(Vector2 direction)
    {
        _animator.SetTrigger("Attack");
        _rb.AddForce(new Vector2(direction.x * jumpForce, jumpForce), ForceMode2D.Impulse);
    }

    void Attack()
    {
        _animator.SetTrigger("Attack");
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
                _rb.AddForce(Vector2.right * 100f);
            }
        }
    }
    
    void LookAtPlayer()
    {
        if (player == null) return;

        float playerDirection = player.position.x - transform.position.x;

        if ((playerDirection > 0 && _sr.flipX) || (playerDirection < 0 && !_sr.flipX))
        {
            _sr.flipX = !_sr.flipX;
        }
    }
    
    bool IsGroundAhead()
    {
        float direction = _sr.flipX ? -1f : 1f;
        Vector2 checkOrigin = groundCheck.position + new Vector3(direction * 0.5f, 0f);
        RaycastHit2D hit = Physics2D.Raycast(checkOrigin, Vector2.down, groundCheckDistance, groundLayer);
        
        return hit.collider != null;
    }
}
