using System.Collections;
using TMPro;
using UnityEngine;

public class scr_WeaponShooting : MonoBehaviour
{
    public Camera playerCamera;
    public Transform weaponTransform;

    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 0.1f;

    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    public float spreadIntensity;
    public float recoilIntensity = 2f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    public AudioClip bulletSound;
    public AudioClip reloadSound;

    private AudioSource audioSource;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    private scr_CharacterController characterController;

    #region - Shooting Modes -
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;
    #endregion

    #region - Awake / Update -
    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        bulletsLeft = magazineSize;

        characterController = GetComponentInParent<scr_CharacterController>();
        audioSource = gameObject.AddComponent<AudioSource>();

        if (weaponTransform == null)
        {
            weaponTransform = transform;
        }
    }

    void Update()
    {
        if (!scr_PlayerStats.isDead && !CollectibleObject.isWin && !PauseMenu.isPaused)
        {
            if (characterController.isSprinting)
            {
                isShooting = false;
                return;
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
            {
                Reload();
            }

            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0 && !isReloading)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (AmmoManager.Instance.ammoDisplay != null)
            {
                AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft}/{magazineSize}";
            }
        }
    }
    #endregion

    #region - Shooting -
    private void FireWeapon()
    {
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        StartCoroutine(ApplyRecoil());

        if (bulletSound != null)
        {
            GameObject soundObject = new GameObject("BulletSound");
            soundObject.transform.position = bulletSpawn.position;
            AudioSource tempAudioSource = soundObject.AddComponent<AudioSource>();
            tempAudioSource.clip = bulletSound;
            tempAudioSource.Play();
            Destroy(soundObject, bulletSound.length);
        }

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private IEnumerator ApplyRecoil()
    {
        Quaternion originalRotation = weaponTransform.localRotation;
        float elapsedTime = 0f;
        float recoilDuration = 0.1f;

        while (elapsedTime < recoilDuration)
        {
            float recoilX = Random.Range(-recoilIntensity, recoilIntensity);
            float recoilY = Random.Range(-recoilIntensity, recoilIntensity);

            Quaternion recoilRotation = Quaternion.Euler(originalRotation.eulerAngles + new Vector3(recoilY, recoilX, 0));
            weaponTransform.localRotation = recoilRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weaponTransform.localRotation = originalRotation;
    }

    private void Reload()
    {
        isReloading = true;

        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = Random.Range(-spreadIntensity, spreadIntensity);
        float y = Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
    #endregion
}