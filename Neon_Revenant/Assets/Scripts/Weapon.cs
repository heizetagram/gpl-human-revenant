using UnityEngine;

public class Weapon : MonoBehaviour {
    public Transform FirePoint => playerController.firePoint;
    public GameObject bulletPrefab;
    public PlayerController playerController;


    void Update() {
        if (Input.GetButtonDown("Fire1") && playerController.isRifleMode) {
            Shoot();
        }
        
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, FirePoint.position, Quaternion.identity);

        int dir = playerController.spriteRenderer.flipX ? -1 : 1;

        bullet.GetComponent<Bullet>().SetDirection(dir);

        if (dir == -1) {
            Vector3 originalScale = bullet.transform.localScale;
            bullet.transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x) * dir, 
                originalScale.y, 
                originalScale.z
            );
        }
    }

}