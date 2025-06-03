using UnityEngine;
using System.Linq;

using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float multiplier = 0.5f;
    public bool horizontalOnly = false;
    public bool calculateInfiniteHorizontalPosition = false;
    public bool calculateInfiniteVerticalPosition = false;
    public bool isInfinite;

    public GameObject camera;

    private Vector3 startPosition;
    private Vector3 startCameraPosition;
    private float length;

    void Start()
    {
        startPosition = transform.position;
        startCameraPosition = camera.transform.position;
        if(isInfinite){
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        CalculateStartPosition();

    }

    void CalculateStartPosition()
    {
        float distX = (camera.transform.position.x - transform.position.x) * multiplier;
        float distY = (camera.transform.position.y - transform.position.y) * multiplier;

        Vector3 tmp = new Vector3(startPosition.x, startPosition.y, startPosition.z);

        if (calculateInfiniteHorizontalPosition)
            tmp.x = transform.position.x + distX;

        if (calculateInfiniteVerticalPosition)
            tmp.y = transform.position.y + distY;

        startPosition = tmp;
    }

    void FixedUpdate()
    {
        Vector3 position = startPosition;

        if (horizontalOnly)
        {
            position.x += multiplier * (camera.transform.position.x - startCameraPosition.x);
        }
        else
        {
            position += multiplier * (camera.transform.position - startCameraPosition);
        }

        transform.position = position;

        if(isInfinite){
            float tmp = camera.transform.position.x * (1-multiplier);
            if (tmp > startPosition.x + length){
                startPosition.x += length;
            }
            else if(tmp < startPosition.x - length){
                startPosition.x -= length;
            }
        }
    }
}