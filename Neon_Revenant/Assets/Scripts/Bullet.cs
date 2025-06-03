using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 20f;
    public int damage = 40;
    public Rigidbody2D rb;

    private int direction = 1;
    private bool directionSet = false;

    public void SetDirection(int dir)
    {
        direction = dir;
        directionSet = true;

        // If rb is already initialized, apply immediately
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
    }

    void Start()
    {
        // If direction was already set before Start, use it
        if (directionSet)
        {
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
        else
        {
            // fallback in case SetDirection wasn't called yet
            rb.linearVelocity = new Vector2(speed, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        IDamageable damageable = hitInfo.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
