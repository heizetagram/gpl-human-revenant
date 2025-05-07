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
    private SpriteRenderer spriteRenderer;
    
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
        
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        
        if (moveInput > 0)
            spriteRenderer.flipX = false; 
        else if (moveInput < 0)
            spriteRenderer.flipX = true; 
        
    }
}
