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
    
    [SerializeField]
    private Transform _bulletFirePoint;

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

        if (_playerAwarenessController.AwareOfPlayer && timeSinceLastFire >= _timeBetweenShots && _enemyHealthController.currentHealthNum != 0)
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

        yield return new WaitForSeconds(0.35f); // Adjust this duration to match your shooting animation duration

        _isShooting = false;
        SetAnimation();
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsShooting", _isShooting);
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _bulletFirePoint.position, Quaternion.identity);
        Vector2 direction = _playerAwarenessController.DirectionToPlayer;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed; 

        // Rotate the bullet to face the direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
