using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        if (player == null) return;

        spriteRenderer.flipX = (transform.position.x > player.position.x);
    }
}