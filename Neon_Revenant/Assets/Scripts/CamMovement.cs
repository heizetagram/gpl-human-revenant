using UnityEngine;

public class CamMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }
}