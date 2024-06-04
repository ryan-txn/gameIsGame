using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    
    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown; // time to change direction
    private float _wallDetectionCooldown; // time to change direction when colliding with wall
    private Camera _camera;
    private bool _isIdle; // Flag to track idle state
    private float _idleTimer; // Timer for idle duration

    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _camera = Camera.main;
        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        SetAnimation();
        if (_isIdle)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0)
            {
                _isIdle = false;
                HandleRandomDirectionChange();
            }
        }
        else
        {
            UpdateTargetDirection();
            SetVelocity();
            UpdateSpriteDirection();
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
        else if (!_isIdle) // Only check for random direction change if not already idling
        {
            HandleRandomDirectionChange();
        }
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
        if (!_isIdle)
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
        if (_targetDirection.x < 0)
        {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_targetDirection.x > 0)
        {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _wallDetectionCooldown -= Time.deltaTime;
       if (collision.gameObject.GetComponent<TilemapCollider2D>() && _wallDetectionCooldown <= 0)
        {
            Debug.Log("Enemy collided with Wall!");
            _targetDirection = - _targetDirection;

            _wallDetectionCooldown = 1f;
        }
    }

    public void OnPlayerLeaveAwareness() // Call this function from PlayerAwarenessController when player leaves awareness zone
    {
        _isIdle = true;
        _idleTimer = 2f; // Set idle timer for 2 seconds
        _targetDirection = - _targetDirection;
    }
}
