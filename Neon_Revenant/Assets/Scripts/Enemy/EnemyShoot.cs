using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header("Movement")]
    public float playerDetectDistance = 2f;
    public LayerMask playerLayer;
    public float moveSpeed = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float minAttackInterval = 2f;
    public float maxAttackInterval = 5f;

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator _animator;

    private bool _movingRight = true;
    private bool _isAttacking = false;
    private float _nextAttackTime = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        SetNextAttackTime();
    }

    void Update()
    {
        if (Time.time >= _nextAttackTime && !_isAttacking)
        {
            StartAttack();
        }

        if (!_isAttacking)
        {
            if (!IsPlayerInFront())
            {
                Patrol();
            }
            else
            {
                _rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            _rb.linearVelocity = Vector2.zero; 
        }

    }

    void Patrol()
    {
        _rb.linearVelocity = new Vector2((_movingRight ? 1 : -1) * moveSpeed, _rb.linearVelocity.y);

        Vector2 checkPos = groundCheck.position + Vector3.right * (_movingRight ? 1f : -1f);
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 1f, groundLayer);

        if (!hit)
        {
            Flip();
        }
    }

    void Flip()
    {
        _movingRight = !_movingRight;
        _sr.flipX = !_sr.flipX;
    }

    void StartAttack()
    {
        _isAttacking = true;
        _animator.SetTrigger("Attack");

        Invoke(nameof(EndAttack), 1f);
    }

    void EndAttack()
    {
        _isAttacking = false;
        SetNextAttackTime();
    }

    void SetNextAttackTime()
    {
        float delay = Random.Range(minAttackInterval, maxAttackInterval);
        _nextAttackTime = Time.time + delay;
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        float direction = _sr.flipX ? -1f : 1f;
        bulletRb.linearVelocity = new Vector2(direction * bulletSpeed, 0f);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction < 0 ? -1 : 1);
            bulletScript.shooterTag = gameObject.tag;
        }
    }
    
    bool IsPlayerInFront()
    {
        Vector2 direction = _movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, playerDetectDistance, playerLayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}
