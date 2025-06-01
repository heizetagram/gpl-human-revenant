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

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    public void TakeDamage(int amount)
    {
        animator.SetTrigger("Hurt");
        playerHealth.TakeDamage(amount);
    }
}