using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyShooter : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private EnemyBullet bulletPrefab;
    [SerializeField] private Transform bulletsParent; // is this supposed to be the enemy that is shooting ?
    [SerializeField] private Transform bulletSpawnPoint; //
    private Enemy enemy;

    [Header(" Settings ")]
    [SerializeField] private float bulletSpeed;
    private bool hasShot;

    public void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void TryShooting()
    {
        if (hasShot)
            return;

        hasShot = true;

        Invoke("Shoot", 0.4f);
    }

    private void Shoot()
    {
        if (enemy.IsDead())
            return;

        Vector3 velocity = bulletSpeed * bulletSpawnPoint.right;

        EnemyBullet bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position,
                                                Quaternion.identity, bulletsParent);
        bulletInstance.Configure(velocity);
    }
}
