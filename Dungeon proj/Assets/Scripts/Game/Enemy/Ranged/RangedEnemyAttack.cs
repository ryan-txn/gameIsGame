using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private float bulletSpeed;

    private float lastFireTime;
    private PlayerAwarenessController _playerAwarenessController;

    private void Awake()
    {
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }

    private void Update()
    {
        float timeSinceLastFire = Time.time - lastFireTime;

        if (_playerAwarenessController.AwareOfPlayer && timeSinceLastFire >= _timeBetweenShots)
        {
            FireBullet();
            lastFireTime = Time.time;
        }
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = _playerAwarenessController.DirectionToPlayer;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed; 

        // Rotate the bullet to face the direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
