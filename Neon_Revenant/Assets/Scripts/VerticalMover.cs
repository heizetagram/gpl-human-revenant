using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    public float moveDistance = 2f; 
    public float moveSpeed = 2f;       

    private Vector3 _startPos;
    private bool _movingUp = true;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        float newY = transform.position.y + (_movingUp ? 1 : -1) * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (_movingUp && newY >= _startPos.y + moveDistance)
            _movingUp = false;
        else if (!_movingUp && newY <= _startPos.y - moveDistance)
            _movingUp = true;
    }
}

