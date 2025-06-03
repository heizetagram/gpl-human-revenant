using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public GameObject laserObject;
    public float shootDuration = 2f;
    public float interval = 3f;

    private float timer = 0f;
    private bool isShooting = false;

    void Start()
    {
        laserObject.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isShooting && timer >= interval)
        {
            StartCoroutine(ShootLaser());
        }
    }

    System.Collections.IEnumerator ShootLaser()
    {
        isShooting = true;
        laserObject.SetActive(true);

        yield return new WaitForSeconds(shootDuration);

        laserObject.SetActive(false);
        timer = 0f;
        isShooting = false;
    }
}