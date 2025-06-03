using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject rifleBulletPrefab;
    public GameObject powerGunBulletPrefab;
    public GameObject sniperBulletPrefab;
    public PlayerController playerController;
    private float lastShotTime = 0f;
    public float rifleCooldown = 0.2f;
    public float powerGunCooldown = 0.3f;
    public float sniperCooldown = 1.0f;


    void Start()
    {
        firePoint = playerController.firePoint;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && CanShoot() && (playerController.isRifleMode || playerController.isPowerGunMode || playerController.isSniperMode))
        {
            Shoot();
        }

    }

    public void Shoot()
    {
        GameObject bulletPrefab = GetEquippedWeaponBullet();
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, null);
        int direction = playerController.spriteRenderer.flipX ? -1 : 1;
        bullet.GetComponent<Bullet>().SetDirection(direction);

        lastShotTime = Time.time;
    }

    public void SwitchWeapon(KeyCode key)
    {
        DisableAllWeaponModes();
        switch (key)
        {
            case KeyCode.Alpha1:
                EquipWeapon(WeaponType.Melee, "");
                break;
            case KeyCode.Alpha2:
                EquipWeapon(WeaponType.Rifle, "isRifleMode");
                UpdateFirePointOffset(0.19f, 0.008f);
                playerController.isRifleMode = true;
                break;
            case KeyCode.Alpha3:
                EquipWeapon(WeaponType.Power, "isPowerGunMode");
                UpdateFirePointOffset(0.168f, 0.024f);
                playerController.isPowerGunMode = true;
                break;
            case KeyCode.Alpha4:
                EquipWeapon(WeaponType.Sniper, "isSniperMode");
                UpdateFirePointOffset(0.1395f, 0.0363f);
                playerController.isSniperMode = true;
                break;
        }
    }


    public void EquipWeapon(WeaponType weaponType, string gunAnimatorVar)
    {
        playerController.equippedWeapon = weaponType;

        if (weaponType != WeaponType.Melee)
        {
            playerController.animator.SetBool(gunAnimatorVar, true);
        }
    }

    void DisableAllWeaponModes()
    {
        playerController.animator.SetBool("isRifleMode", false);
        playerController.animator.SetBool("isPowerGunMode", false);
        playerController.animator.SetBool("isSniperMode", false);
        playerController.isRifleMode = false;
        playerController.isPowerGunMode = false;
        playerController.isSniperMode = false;
    }

    void UpdateFirePointOffset(float offsetX, float offsetY)
    {
        if (playerController == null || firePoint == null || playerController.spriteRenderer == null)
            return;

        int direction = playerController.spriteRenderer.flipX ? -1 : 1;

        Vector3 newLocalPos = new Vector3(offsetX * direction, offsetY, firePoint.localPosition.z);
        firePoint.localPosition = newLocalPos;
    }


    GameObject GetEquippedWeaponBullet()
    {
        if (playerController.isRifleMode)
        {
            return rifleBulletPrefab;
        }
        else if (playerController.isPowerGunMode)
        {
            return powerGunBulletPrefab;
        }
        else if (playerController.isSniperMode)
        {
            return sniperBulletPrefab;
        }
        else
            return null;
    }

    float GetCurrentWeaponCooldown()
    {
        if (playerController.isRifleMode)
            return rifleCooldown;
        else if (playerController.isPowerGunMode)
            return powerGunCooldown;
        else if (playerController.isSniperMode)
            return sniperCooldown;
        else
            return 0f;
    }
    
    bool CanShoot()
    {
        float cooldown = GetCurrentWeaponCooldown();
        return Time.time >= lastShotTime + cooldown;
    }

}

public enum WeaponType
{
    Melee,
    Rifle,
    Power,
    Sniper
}
