using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        if (player == null) return;

        _spriteRenderer.flipX = (transform.position.x > player.position.x);
    }
}