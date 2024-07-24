using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JamMeleeMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private bool _isIdle;

    private Vector2 _targetDirection;

    private float _changeDirectionCooldown;
    private float _wallDetectionCooldown; // time to change direction when colliding with wall
    private float _idleTimer;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;

    private RangedEnemyAttack _rangedEnemyAttack;

    private HealthController _healthController;

    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _animator = GetComponentInChildren<Animator>();
        
        _healthController = GetComponent<HealthController>();
    }

    private void FixedUpdate()
    {
        SetAnimation();
        //UpdateTargetDirection();
        if (_isIdle)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0)
            {
                _isIdle = false;
            }
        }
        else
        {
            SetVelocity();
            UpdateSpriteDirection();
            HandleRandomDirectionChange();
        }
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsIdle", _isIdle);
    }

    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
            _isIdle = false; // Reset idle timer if player is detected
        }
        else if (!_isIdle && !IsShooting()) // Only check for random direction change if not already idling or shooting
        {
            HandleRandomDirectionChange();
        }
    }

    private bool IsShooting()
    {
        return _rangedEnemyAttack != null && _rangedEnemyAttack.IsShooting;
    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(1f, 5f);
        }
    }

    private void SetVelocity() 
    {
        if (IsShooting())
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else if (!_isIdle)
        {
            _rigidbody.velocity = _targetDirection.normalized * _speed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero; // Set velocity to zero while idling
        }
    }

    private void UpdateSpriteDirection()
    {
        Vector2 _playerDirection = _playerAwarenessController.DirectionToPlayer;

        if (_playerDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_playerDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Turn the other way if walk into wall
    private void OnCollisionStay2D(Collision2D collision)
    {
        _wallDetectionCooldown -= Time.deltaTime;
        if (collision.gameObject.GetComponent<TilemapCollider2D>() && _wallDetectionCooldown <= 0)
        {
            _targetDirection = -_targetDirection;
            _wallDetectionCooldown = 1f;
        }
    }

    public void OnPlayerLeaveAwareness() // Call this function from PlayerAwarenessController when player leaves awareness zone
    {
        if (!IsShooting()) // Only go idle if not shooting
        {
            _isIdle = true;
            _idleTimer = 2f; // Set idle timer for 2 seconds
            _targetDirection = -_targetDirection;
        }
    }
}
