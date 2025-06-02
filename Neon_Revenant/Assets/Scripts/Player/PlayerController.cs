using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 1f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    public SpriteRenderer spriteRenderer;
    private PlayerHealth playerHealth;
    private bool isGrounded;
    // From test-3
    private float moveInput;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public bool isRifleMode = false;
    private bool jumpRequested = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        // Weapons mechanics
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            animator.SetBool("isRifleMode", true);
            isRifleMode = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            animator.SetBool("isRifleMode", false);
            isRifleMode = false;
        }

        if (Input.GetButtonDown("Fire1") && isRifleMode)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            int direction = spriteRenderer.flipX ? -1 : 1;
            bullet.GetComponent<Bullet>().SetDirection(direction);
        }
        
        FlipPlayer();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }

        animator.SetBool("isRunning", moveInput != 0);
        animator.SetBool("isAirborne", !isGrounded);
        
    }

    void FlipPlayer()
    {
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
            firePoint.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
            firePoint.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    void FixedUpdate()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(groundLayer);
        filter.useTriggers = false;

        RaycastHit2D[] results = new RaycastHit2D[1];
        int hitCount = rb.Cast(Vector2.down, filter, results, 0.1f);

        isGrounded = hitCount > 0;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }

    }
    
    public void TakeDamage(int amount)
    {
        animator.SetTrigger("Hurt");
        playerHealth.TakeDamage(amount);
    }
}