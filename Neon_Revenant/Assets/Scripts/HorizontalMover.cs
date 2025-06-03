using System.Collections.Generic;
using UnityEngine;

public class HorizontalMover : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 lastPos;
    private bool movingRight = true;

    private Rigidbody2D rb;
    private List<Rigidbody2D> objectsOnPlatform = new List<Rigidbody2D>();

    void Start()
    {
        startPos = transform.position;
        lastPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float direction = movingRight ? 1 : -1;
        Vector3 newPos = transform.position + new Vector3(direction * moveSpeed * Time.fixedDeltaTime, 0, 0);

        if (movingRight && newPos.x >= startPos.x + moveDistance)
            movingRight = false;
        else if (!movingRight && newPos.x <= startPos.x - moveDistance)
            movingRight = true;

        Vector3 delta = newPos - transform.position;

        rb.MovePosition(newPos);

        // Alle Körper auf Plattform mitbewegen
        foreach (Rigidbody2D body in objectsOnPlatform)
        {
            body.MovePosition(body.position + new Vector2(delta.x, 0));
        }

        lastPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.rigidbody != null)
        {
            if (!objectsOnPlatform.Contains(collision.rigidbody))
                objectsOnPlatform.Add(collision.rigidbody);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
            objectsOnPlatform.Remove(collision.rigidbody);
    }
}