using System.Collections.Generic;
using UnityEngine;

public class HorizontalMover : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 _startPos;
    private Vector3 _lastPos;
    private bool _movingRight = true;

    private Rigidbody2D _rb;
    private List<Rigidbody2D> _objectsOnPlatform = new List<Rigidbody2D>();

    void Start()
    {
        _startPos = transform.position;
        _lastPos = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float direction = _movingRight ? 1 : -1;
        Vector3 newPos = transform.position + new Vector3(direction * moveSpeed * Time.fixedDeltaTime, 0, 0);

        if (_movingRight && newPos.x >= _startPos.x + moveDistance)
            _movingRight = false;
        else if (!_movingRight && newPos.x <= _startPos.x - moveDistance)
            _movingRight = true;

        Vector3 delta = newPos - transform.position;

        _rb.MovePosition(newPos);

        foreach (Rigidbody2D body in _objectsOnPlatform)
        {
            body.MovePosition(body.position + new Vector2(delta.x, 0));
        }

        _lastPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.rigidbody != null)
        {
            if (!_objectsOnPlatform.Contains(collision.rigidbody))
                _objectsOnPlatform.Add(collision.rigidbody);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
            _objectsOnPlatform.Remove(collision.rigidbody);
    }
}