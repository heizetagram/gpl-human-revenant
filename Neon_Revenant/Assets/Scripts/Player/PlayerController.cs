using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 1f;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private PlayerHealth playerHealth;
    private bool isGrounded;
    private float moveInput;
    public Transform firePoint;
    public bool isRifleMode = false;
    private bool jumpRequested = false;
    public TextMeshProUGUI unlockMessage;
    public bool hasPowerGun;
    public bool isPowerGunMode = false;
    public bool hasSniper;
    public bool isSniperMode = false;
    public WeaponType equippedWeapon;
    public Weapon weapon;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
        weapon = weapon = GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
            spriteRenderer.flipX = false;
        else if (moveInput < 0)
            spriteRenderer.flipX = true;

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weapon.SwitchWeapon(KeyCode.Alpha1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                weapon.SwitchWeapon(KeyCode.Alpha2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && hasPowerGun)
            {
                weapon.SwitchWeapon(KeyCode.Alpha3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && hasSniper)
            {
                weapon.SwitchWeapon(KeyCode.Alpha4);
            }

        }
        /*
        if (Input.GetButtonDown("Fire1") && (isRifleMode || isPowerGunMode || isSniperMode))
        {
            weapon.Shoot();
        }*/

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

    void ShowUnlockMessage(string message)
    {
        unlockMessage.text = message;
        unlockMessage.gameObject.SetActive(true);
        StartCoroutine(HideUnlockMessage());
    }

    IEnumerator HideUnlockMessage()
    {
        yield return new WaitForSeconds(2f);
        unlockMessage.gameObject.SetActive(false);
    }


    public void AssignGuns(int totalBarcodes)
    {
        if (!hasPowerGun && totalBarcodes >= 10)
        {
            hasPowerGun = true;
            ShowUnlockMessage("Power Gun Unlocked");
        }

        if (!hasSniper && totalBarcodes >= 15)
        {
            hasSniper = true;
            ShowUnlockMessage("Sniper Unlocked");
        }
    }
}

