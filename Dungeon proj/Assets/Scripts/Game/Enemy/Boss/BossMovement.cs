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
    private GameObject _enemySpawnerPrefab;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private float _spawnIntervalPhaseOne;

    [SerializeField]
    private float _spawnIntervalPhaseTwo;

    private float _lastSpawnTime;

    private List<GameObject> _spawners;

    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _animator = GetComponentInChildren<Animator>();
        _rangedEnemyAttack = GetComponent<RangedEnemyAttack>();

        //for each spawn point, create a spawner to add to _spawners list
        _spawners = new List<GameObject>();
        foreach (var spawnPoint in _spawnPoints)
        {
            GameObject gummySpawner = Instantiate(_enemySpawnerPrefab, spawnPoint.position, spawnPoint.rotation);
            _spawners.Add(gummySpawner);
        }
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

            SpawnEnemies();

    }

    private void PhaseTwo()
    {
        _isIdle = false; // Boss starts moving towards player
        UpdateTargetDirection();
        SetVelocity();

        SpawnEnemies();
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
        foreach (var spawner in _spawners)
        {
            var enemySpawner = spawner.GetComponent<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.enabled = true; // Activate the spawner
            }
        }
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsIdle", _isIdle);
    }
}
