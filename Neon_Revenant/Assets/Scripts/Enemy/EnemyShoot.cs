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

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private bool movingRight = true;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        SetNextAttackTime();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime && !isAttacking)
        {
            StartAttack();
        }

        if (!isAttacking)
        {
            if (!IsPlayerInFront())
            {
                Patrol();
            }
            else
            {
                rb.linearVelocity = Vector2.zero; // Spieler erkannt → stehen bleiben
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero; 
        }

    }

    void Patrol()
    {
        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb.linearVelocity.y);

        Vector2 checkPos = groundCheck.position + Vector3.right * (movingRight ? 1f : -1f);
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, 1f, groundLayer);

        if (!hit)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        sr.flipX = !sr.flipX;
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        Invoke(nameof(EndAttack), 1f); // Ende des Angriffs nach 1 Sekunde
    }

    void EndAttack()
    {
        isAttacking = false;
        SetNextAttackTime();
    }

    void SetNextAttackTime()
    {
        float delay = Random.Range(minAttackInterval, maxAttackInterval);
        nextAttackTime = Time.time + delay;
    }

    // Wird von Animation Event aufgerufen
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        float direction = sr.flipX ? -1f : 1f;
        bulletRb.linearVelocity = new Vector2(direction * bulletSpeed, 0f);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction < 0 ? -1 : 1);
            //bulletScript.shooterTag = gameObject.tag; // z.B. "Enemy"
        }
    }
    
    bool IsPlayerInFront()
    {
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, playerDetectDistance, playerLayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}
