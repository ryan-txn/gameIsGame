using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private bool _isIdle;
    private Vector2 _targetDirection;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private RangedEnemyAttack _rangedEnemyAttack;
    private HealthController _healthController;

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private float _spawnIntervalPhaseOne;

    [SerializeField]
    private float _spawnIntervalPhaseTwo;

    private float _lastSpawnTime;

    private Animator _animator;

    [SerializeField]
    private LayerMask _enemyLayerMask; // Layer mask for enemies for death explosion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _animator = GetComponentInChildren<Animator>();
        _rangedEnemyAttack = GetComponent<RangedEnemyAttack>();
        _healthController = GetComponent<HealthController>();
    }

    void FixedUpdate()
    {
        SetAnimation();

        // Different boss phases if half health
        if (_healthController.RemainingHealthPercentage > 0.5f)
        {
            PhaseOne();
        }
        else
        {
            PhaseTwo();
        }

    }

    private void PhaseOne()
    {
        _isIdle = true; // Boss is stationary

        if (Time.time - _lastSpawnTime >= _spawnIntervalPhaseOne)
        {
            SpawnEnemies();
            _lastSpawnTime = Time.time;
        }

    }

    private void PhaseTwo()
    {
        _isIdle = false; // Boss starts moving towards player
        UpdateTargetDirection();
        SetVelocity();

        if (Time.time - _lastSpawnTime >= _spawnIntervalPhaseTwo)
        {
            SpawnEnemies();
            _lastSpawnTime = Time.time;
        }
    }

    private void SetVelocity() 
    {
        if (!_isIdle)
        {
            _rigidbody.velocity = _targetDirection.normalized * _speed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero; // Set velocity to zero while idle
        }
    }

    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
            _isIdle = false; // Reset idle timer if player is detected
        }
    }

    private void SpawnEnemies()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            Instantiate(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
    
    //Called under healthcontroller onDied() Event
    public void DeathExplosion()
    {
        float _explosionRadius = 50.0f;

        //Hit enemies in the set explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collider detected: " + collider.gameObject.name);
            var enemy = collider.GetComponent<EnemyMovement>();

            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            HealthController healthController = collider.GetComponent<HealthController>();

            if (healthController != null && rb != null && enemy != null)
            {
                healthController.TakeDamage(50f);
            }
        }
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsIdle", _isIdle);
    }
}
