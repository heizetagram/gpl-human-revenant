using UnityEngine;
using System.Linq;

public class ParallaxLooper : MonoBehaviour
{
    public Transform[] backgrounds;        // Alle BGs
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;

    private Vector3 _lastCameraPos;
    private float _backgroundWidth;

    void Start()
    {
        _lastCameraPos = cameraTransform.position;
        _backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        Vector3 delta = cameraTransform.position - _lastCameraPos;
        transform.position += new Vector3(delta.x * parallaxFactor, 0, 0);
        _lastCameraPos = cameraTransform.position;

        backgrounds = backgrounds.OrderBy(bg => bg.position.x).ToArray();

        Transform leftMost = backgrounds[0];
        Transform rightMost = backgrounds[backgrounds.Length - 1];

        if (cameraTransform.position.x > rightMost.position.x - _backgroundWidth)
        {
            leftMost.position = new Vector3(rightMost.position.x + _backgroundWidth, leftMost.position.y, leftMost.position.z);
        }

        if (cameraTransform.position.x < leftMost.position.x + _backgroundWidth)
        {
            rightMost.position = new Vector3(leftMost.position.x - _backgroundWidth, rightMost.position.y, rightMost.position.z);
        }
    }
}