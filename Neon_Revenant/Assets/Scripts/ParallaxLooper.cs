using UnityEngine;
using System.Linq;

public class ParallaxLooper : MonoBehaviour
{
    public Transform[] backgrounds;        // Alle BGs
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;

    private Vector3 lastCameraPos;
    private float backgroundWidth;

    void Start()
    {
        lastCameraPos = cameraTransform.position;
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        Vector3 delta = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(delta.x * parallaxFactor, 0, 0);
        lastCameraPos = cameraTransform.position;

        // Sortiere nach x-Position
        backgrounds = backgrounds.OrderBy(bg => bg.position.x).ToArray();

        // Linkester und rechter BG
        Transform leftMost = backgrounds[0];
        Transform rightMost = backgrounds[backgrounds.Length - 1];

        // Wenn Kamera zu weit rechts ist → verschiebe ganz linken BG nach rechts
        if (cameraTransform.position.x > rightMost.position.x - backgroundWidth)
        {
            leftMost.position = new Vector3(rightMost.position.x + backgroundWidth, leftMost.position.y, leftMost.position.z);
        }

        // Wenn Kamera zu weit links ist → verschiebe ganz rechten BG nach links
        if (cameraTransform.position.x < leftMost.position.x + backgroundWidth)
        {
            rightMost.position = new Vector3(leftMost.position.x - backgroundWidth, rightMost.position.y, rightMost.position.z);
        }
    }
}