using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private bool _bossIsActivated;

    [SerializeField]
    private float _speed;
    private bool _isIdle;
    private bool _isTransitioning; //transitioning from phase 1 to 2
    private bool _transitionDone;
    private bool _isDead;
    private Vector2 _targetDirection;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;

    [SerializeField]
    private RangedEnemyAttack _rangedEnemyAttackPhaseOne;
    [SerializeField]
    private RangedEnemyAttack _rangedEnemyAttackPhaseTwo;

    private HealthController _healthController;

    //Enemy Spawning
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private float _spawnIntervalPhaseOne;

    [SerializeField]
    private float _spawnIntervalPhaseTwo;

    [SerializeField]
    private float _maxNumOfGummyToSpawn = 20;

    private float _lastSpawnTime;

    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _animator = GetComponentInChildren<Animator>();
        _healthController = GetComponent<HealthController>();

        // Enable phase one attack script and disable phase two attack script at the start
        _rangedEnemyAttackPhaseOne.enabled = true;
        _rangedEnemyAttackPhaseTwo.enabled = false;
    }

    void FixedUpdate()
    {
        SetAnimation();

        if (_bossIsActivated)
        {
            if (_healthController._currentHealth == 0)
            {
                _isDead = true;
            }

            // Different boss phases if half health
            if (_healthController.RemainingHealthPercentage > 0.5f)
            {
                PhaseOne();
            }
            else if (!_transitionDone)
            {
                StartCoroutine(TransitionToPhaseTwo());
            }
            else
            {
                PhaseTwo();
            }
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

    private IEnumerator TransitionToPhaseTwo()
    {
        //Turn off phase one bullet firing
        _rangedEnemyAttackPhaseOne.enabled = false;

        _isTransitioning = true;
        _healthController.IsInvincible = true;

        yield return new WaitForSeconds(2.5f);

        _isTransitioning = false;
        _healthController.IsInvincible = false;

        _transitionDone = true;
    }

    private void PhaseTwo()
    {
        Debug.Log("Boss Phase 2 started");

        _isIdle = false; // Boss starts moving towards player
        UpdateTargetDirection();
        UpdateSpriteDirection();
        SetVelocity();

        _rangedEnemyAttackPhaseTwo.enabled = true;

        if ((Time.time - _lastSpawnTime >= _spawnIntervalPhaseTwo) && !_isDead)
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

    private void UpdateSpriteDirection()
    {
        if (_targetDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_targetDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void SpawnEnemies()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            int currentEnemyCount = EnemyCounter.GetEnemyCount();

            if (currentEnemyCount < _maxNumOfGummyToSpawn)
            {
                Instantiate(_enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                EnemyCounter.SetEnemies(currentEnemyCount += 1);
            }
            else
            {
                Debug.Log("Max Gummy number reached");
            }
        }
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsIdle", _isIdle);
        _animator.SetBool("IsTransitioning", _isTransitioning);
    }

    public void ActivateBoss()
    {
        _bossIsActivated = true;
        FindObjectOfType<AudioManager>().PlayMusic("Boss 1 music");
        Debug.Log("Boss is activated, now playing boss music");
    }

    public void FadeBossMusic()
    {
        FindObjectOfType<AudioManager>().FadeOutMusic("Boss 1 music", 5);
    }
}
