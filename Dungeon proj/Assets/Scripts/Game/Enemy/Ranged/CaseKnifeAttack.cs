using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CaseKnifeAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private Transform _bulletFirePoint;

    [SerializeField]
    private float _shootAnimationDuration;

    [SerializeField]
    private bool _isSpreadShot;

    [SerializeField] 
    private bool _isLeft;

    [SerializeField]
    private int numberOfProjectiles = 12; // Total number of projectiles including the main one
    [SerializeField]
    private float spreadAngle = 30f; // Angle between each projectile

    private float lastFireTime;
    private PlayerAwarenessController _playerAwarenessController;
    private HealthController _enemyHealthController;
    private Animator _animator;
    private bool _isShooting = false;
    public bool IsShooting => _isShooting;

    private void Awake()
    {
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _enemyHealthController = GetComponent<HealthController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float timeSinceLastFire = Time.time - lastFireTime;

        if (_playerAwarenessController.AwareOfPlayer && timeSinceLastFire >= _timeBetweenShots && _enemyHealthController._currentHealth != 0)
        {
            StartCoroutine(ShootCoroutine());
            lastFireTime = Time.time;
        }
    }

    private IEnumerator ShootCoroutine()
    {
        _isShooting = true;
        SetAnimation();

        FireBullet();

        yield return new WaitForSeconds(_shootAnimationDuration); // Adjust this duration to match your shooting animation duration

        _isShooting = false;
        SetAnimation();
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsShooting", _isShooting);
    }

    private void FireBullet()
    {
        if (_isSpreadShot)
        {
            FireSpreadShot();
        }
        else
        {
            FireSingleShot();
        }
    }

    private void FireSingleShot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _bulletFirePoint.position, Quaternion.identity);
        Vector2 direction = _playerAwarenessController.DirectionToPlayer;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        // Rotate the bullet to face the direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void FireSpreadShot()
    {
        Vector2 directionToPlayer;
        if (_isLeft)
        {
            directionToPlayer = new Vector2(1, -1);
        }
        else
        {
            directionToPlayer = new Vector2(-1, -1);
        }
        directionToPlayer = directionToPlayer.normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angleOffset = (i - numberOfProjectiles / 2) * spreadAngle;
            float angle = baseAngle + angleOffset;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = Instantiate(_bulletPrefab, _bulletFirePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            // Rotate the bullet to face the direction of travel
            float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bulletAngle += 90;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, bulletAngle));
        }
    }

}
