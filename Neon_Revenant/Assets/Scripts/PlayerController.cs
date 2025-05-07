using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public SpriteRenderer spriteRenderer;
    public Sprite rifleSprite;
    public Sprite normalSprite;
    public bool isRifleMode = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        animator.SetBool("isRunning", moveInput != 0);
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Weapons mechanics
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            animator.SetBool("isRifleMode", true);
            isRifleMode = true;
            spriteRenderer.sprite = rifleSprite;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            animator.SetBool("isRifleMode", false);
            isRifleMode = false;
            spriteRenderer.sprite = normalSprite;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        //
        
        FlipPlayer();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        animator.SetBool("isRunning", moveInput != 0);
        animator.SetBool("isAirborne", !isGrounded);
        
    }

    void FlipPlayer() {
        if (moveInput > 0) {
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
    
    public void TakeDamage()
    {
        animator.SetTrigger("Hurt");
        
    }
}
