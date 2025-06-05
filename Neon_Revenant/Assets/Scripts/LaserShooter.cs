using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public GameObject laserObject;
    public float shootDuration = 2f;
    public float interval = 3f;

    private float _timer = 0f;
    private bool _isShooting = false;

    void Start()
    {
        laserObject.SetActive(false);
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (!_isShooting && _timer >= interval)
        {
            StartCoroutine(ShootLaser());
        }
    }

    System.Collections.IEnumerator ShootLaser()
    {
        _isShooting = true;
        laserObject.SetActive(true);

        yield return new WaitForSeconds(shootDuration);

        laserObject.SetActive(false);
        _timer = 0f;
        _isShooting = false;
    }
}