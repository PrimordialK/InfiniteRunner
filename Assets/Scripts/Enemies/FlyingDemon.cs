using UnityEngine;

public class FlyDemon : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRange = 8f;
    [SerializeField] private float fireCooldown = 2f;

    private Transform playerTransform;
    private float _nextFireTime;

    protected override void Start()
    {
        base.Start();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogWarning("FlyDemon: No GameObject found with tag 'Player'.");
    }

    void Update()
    {
        if (playerTransform == null) return;

        float direction = playerTransform.position.x - transform.position.x;
        FaceDirection(direction);

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= fireRange && Time.time >= _nextFireTime)
            FireProjectile();
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("FlyDemon: No projectile prefab assigned.");
            return;
        }

        _nextFireTime = Time.time + fireCooldown;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        Vector2 direction = (playerTransform.position - spawnPosition).normalized;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        if (projectile.TryGetComponent(out DemonProjectile demonProjectile))
            demonProjectile.Launch(direction);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}