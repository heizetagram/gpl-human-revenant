using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 20f;
    public int damage = 40;
    public Rigidbody2D rb;

    private int direction = 1;

    public void SetDirection(int dir)
    {
        direction = dir;
    }

    void Start()
    {
        rb.linearVelocity = new Vector2(direction * speed, 0f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}