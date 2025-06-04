using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 40;
    public Rigidbody2D rb;
    public float timeToLive = 2f;
    public string shooterTag; // Neu: Wer hat geschossen?

    private int direction = 1;
    private bool directionSet = false;
    private Vector2 startPosition;

    public void SetDirection(int dir)
    {
        direction = dir;
        directionSet = true;

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        }
    }

    void Start()
    {
        startPosition = transform.position;

        if (directionSet)
            rb.linearVelocity = new Vector2(direction * speed, 0f);
        else
            rb.linearVelocity = new Vector2(speed, 0f);

        Destroy(gameObject, timeToLive);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Nicht den Schützen selbst treffen
        if (hitInfo.CompareTag(shooterTag)) return;

        IDamageable damageable = hitInfo.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float distanceTraveled = Vector2.Distance(startPosition, transform.position);

            float maxEffectiveRange = 10f;
            float minDamageMultiplier = 0.3f;

            float damageMultiplier = Mathf.Max(minDamageMultiplier, 1 - (distanceTraveled / maxEffectiveRange));
            int finalDamage = Mathf.RoundToInt(damage * damageMultiplier);

            damageable.TakeDamage(finalDamage);
            Debug.Log($"Hit {hitInfo.name} for {finalDamage} damage.");
            Destroy(gameObject);
        }
    }
}