using UnityEngine;

public class VerticalMover : MonoBehaviour
{
    public float moveDistance = 2f; 
    public float moveSpeed = 2f;       

    private Vector3 startPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = transform.position.y + (movingUp ? 1 : -1) * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (movingUp && newY >= startPos.y + moveDistance)
            movingUp = false;
        else if (!movingUp && newY <= startPos.y - moveDistance)
            movingUp = true;
    }
}

