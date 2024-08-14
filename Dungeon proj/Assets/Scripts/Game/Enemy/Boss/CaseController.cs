using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class CaseController : MonoBehaviour
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
    private RangedEnemyAttack _leftHandAttack;
    [SerializeField]
    private RangedEnemyAttack _rightHandAttack;
    [SerializeField]
    private CaseKnifeAttack _leftHandSweep;
    [SerializeField]
    private CaseKnifeAttack _rightHandSweep;

    [SerializeField]
    private RangedEnemyAttack _rangedEnemyAttackPhaseTwo;

    private HealthController _healthController;

    //Enemy Spawning
    [SerializeField]
    private List<GameObject> _enemyList;
    
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

    private Coroutine _currentPhaseCoroutine;

    private CapsuleCollider2D _capsuleCollider2D;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _animator = GetComponentInChildren<Animator>();
        _healthController = GetComponent<HealthController>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        // Enable phase one attack script and disable phase two attack script at the start
        _leftHandAttack.enabled = false;
        _rightHandAttack.enabled = false;

        _rangedEnemyAttackPhaseTwo.enabled = false;
    }

    void FixedUpdate()
    {
        SetAnimation();

        if (_bossIsActivated)
        {
            Debug.Log(_healthController._currentHealth);
            if (_healthController._currentHealth == 0)
            {
                _isDead = true;
            }

            // Different boss phases if half health
            if (_healthController.RemainingHealthPercentage > 0.5f)
            {
                PhaseOne();
                Debug.Log("Case in Phase 1");
            }
            else if (!_transitionDone)
            {
                StartCoroutine(TransitionToPhaseTwo());
                Debug.Log("Case transitioning");
            }
            else
            {
                PhaseTwo();
                Debug.Log("Case in Phase 2");
            }
        }
    }

    private void PhaseOne()
    {
        _isIdle = true; // Boss is stationary

        if (_currentPhaseCoroutine == null) // Ensure no duplicate coroutines
        {
            _currentPhaseCoroutine = StartCoroutine(PhaseOneAttackSequence());
        }

        if (Time.time - _lastSpawnTime >= _spawnIntervalPhaseOne)
        {
            //SpawnEnemies();
            _lastSpawnTime = Time.time;
        }
    }

    private IEnumerator PhaseOneAttackSequence()
    {
        while (_healthController.RemainingHealthPercentage > 0.5f && !_isDead)
        {
            // Start Attack 1 Animation (1.33 * 3 = 4 seconds)
            _animator.SetBool("Attack1", true);

            yield return new WaitForSeconds(.5f);
            _leftHandAttack.enabled = true;

            yield return new WaitForSeconds(.5f);
            _rightHandAttack.enabled = true;

            yield return new WaitForSeconds(2.5f);
            _animator.SetBool("Attack1", false);
            _animator.SetBool("Attack2", true);

            yield return new WaitForSeconds(0.5f);
            _leftHandAttack.enabled = false;
            _rightHandAttack.enabled = false;




            // Attack 2 Animation (1.66 * 3 = 5 seconds)
            yield return new WaitForSeconds(.5f);
            _leftHandSweep.enabled = true;

            yield return new WaitForSeconds(.833f);
            _rightHandSweep.enabled = true;

            yield return new WaitForSeconds(3.167f);
            _animator.SetBool("Attack2", false);
            _animator.SetBool("IsIdle", true);


            yield return new WaitForSeconds(0.5f);
            _leftHandSweep.enabled = false;
            _rightHandSweep.enabled = false;


            // Idle
            yield return new WaitForSeconds(2f);
            _animator.SetBool("IsIdle", false);

        }

        // Reset the coroutine reference when done
        _currentPhaseCoroutine = null;
    }

    private IEnumerator TransitionToPhaseTwo()
    {
        //Turn off phase one bullet firing
        _leftHandAttack.enabled = false;
        _rightHandAttack.enabled = false;

        _isTransitioning = true;
        _healthController.IsInvincible = true;

        yield return new WaitForSeconds(3f);

        _isTransitioning = false;
        _healthController.IsInvincible = false;

        _transitionDone = true;

        // Set the offset to (0, 0)
        _capsuleCollider2D.offset = Vector2.zero;

        // Set the size to (1, 3)
        _capsuleCollider2D.size = new Vector2(1f, 1.2f);

        // Set the direction to vertical
        _capsuleCollider2D.direction = CapsuleDirection2D.Vertical; // Vertical direction
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
                int index = Random.Range(0, _enemyList.Count);
                _enemyPrefab = _enemyList[index];
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
        FindObjectOfType<AudioManager>().PlayMusic("Boss 3");
        Debug.Log("Boss is activated, now playing boss music");
    }

    public void FadeBossMusic()
    {
        FindObjectOfType<AudioManager>().FadeOutMusic("Boss 3 ", 5);
    }
}
