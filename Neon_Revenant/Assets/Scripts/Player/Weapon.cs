using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject fakeGlow;
    public GameObject rifleBulletPrefab;
    public GameObject powerGunBulletPrefab;
    public GameObject sniperBulletPrefab;
    public PlayerController playerController;
    private SpriteRenderer fakeGlowRenderer;
    public GameObject meleeIconGO;
    public GameObject rifleIconGO;
    public GameObject powerGunIconGO;
    public GameObject sniperIconGO;
    private float _lastShotTime = 0f;
    public float rifleCooldown = 0.2f;
    public float powerGunCooldown = 0.3f;
    public float sniperCooldown = 1.0f;


    void Start()
    {
        firePoint = playerController.firePoint;
        
        if (fakeGlow != null)
            fakeGlowRenderer = fakeGlow.GetComponent<SpriteRenderer>();
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
        bullet.GetComponent<Bullet>().shooterTag = gameObject.tag;
        _lastShotTime = Time.time;


        if (fakeGlowRenderer != null)
        {
            if (playerController.equippedWeapon == WeaponType.Power)
                fakeGlowRenderer.color = Color.cyan;
            else
                fakeGlowRenderer.color = Color.yellow;
        }

        if (fakeGlow != null)
            StartCoroutine(FlashFakeGlow());
    }

    private IEnumerator FlashFakeGlow()
    {
        fakeGlow.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        fakeGlow.SetActive(false);
    }



    public void SwitchWeapon(KeyCode key)
    {
        DisableAllWeaponModes();
        switch (key)
        {
            case KeyCode.Alpha1:
                EquipWeapon(WeaponType.Melee, "");
                UpdateWeaponIcons(WeaponType.Melee);
                break;
            case KeyCode.Alpha2:
                EquipWeapon(WeaponType.Rifle, "isRifleMode");
                UpdateFirePointOffset(0.19f, 0.008f);
                playerController.isRifleMode = true;
                UpdateWeaponIcons(WeaponType.Rifle);
                break;
            case KeyCode.Alpha3:
                EquipWeapon(WeaponType.Power, "isPowerGunMode");
                UpdateFirePointOffset(0.1814f, 0.0309f);
                playerController.isPowerGunMode = true;
                UpdateWeaponIcons(WeaponType.Power);
                break;
            case KeyCode.Alpha4:
                EquipWeapon(WeaponType.Sniper, "isSniperMode");
                UpdateFirePointOffset(0.1821f, 0.0363f);
                playerController.isSniperMode = true;
                UpdateWeaponIcons(WeaponType.Sniper);
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
        return Time.time >= _lastShotTime + cooldown;
    }
    
    private void UpdateWeaponIcons(WeaponType weaponType)
    {
        meleeIconGO.SetActive(weaponType == WeaponType.Melee);
        rifleIconGO.SetActive(weaponType == WeaponType.Rifle);
        powerGunIconGO.SetActive(weaponType == WeaponType.Power);
        sniperIconGO.SetActive(weaponType == WeaponType.Sniper);
    }


}

public enum WeaponType
{
    Melee,
    Rifle,
    Power,
    Sniper
}


