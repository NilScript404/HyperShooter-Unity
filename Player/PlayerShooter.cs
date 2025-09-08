using UnityEngine;
using System;

public class PlayerShooter : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject shootingLine;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition; // the right hand pistol is injected from the editor 
    [SerializeField] private Transform bulletsParent;

    [Header(" Settings ")]
    [SerializeField] private float bulletSpeed;
    private bool canShoot;

    [Header(" Actions ")]
    public static Action onShot;

    void Awake()
    {
        PlayerMovement.OnEnterWarzone += EnteredWarzoneCallback;
        PlayerMovement.OnExitWarzone += ExitWarzoneCallback;
        PlayerMovement.onDeath += DeathCallback;
    }

    void OnDestroy()
    {
        PlayerMovement.OnEnterWarzone -= EnteredWarzoneCallback;
        PlayerMovement.OnExitWarzone -= ExitWarzoneCallback;
        PlayerMovement.onDeath -= DeathCallback;
    }

    void Start()
    {
        shootingLine.SetActive(false);
    }

    void Update()
    {
        if (canShoot)
        {
            ManageShooting();
        }
    }

    private void ManageShooting()
    {
        // 0 left click, 1 right click, 2 middle mouse click
        if (Input.GetMouseButtonDown(0) && UiBulletContainer.instance.CanShoot())
            Shoot();
    }

    private void Shoot()
    {
        Vector3 direction = bulletSpawnPosition.right;
        direction.z = 0;

        Bullet bulletInstance = Instantiate(bulletPrefab, bulletSpawnPosition.position,
                                                Quaternion.identity, bulletsParent);

        bulletInstance.Configure(direction * bulletSpeed);
        // UnityEditor.EditorApplication.isPaused = true;

        onShot?.Invoke();
    }

    private void EnteredWarzoneCallback()
    {
        SetShootingLineVisibility(true);
        canShoot = true;
    }

    private void ExitWarzoneCallback()
    {
        SetShootingLineVisibility(false);
        canShoot = false;
    }

    private void SetShootingLineVisibility(bool visibility)
    {
        shootingLine.SetActive(visibility);
    }

    private void DeathCallback()
    {
        SetShootingLineVisibility(false);
        canShoot = false;
    }
}