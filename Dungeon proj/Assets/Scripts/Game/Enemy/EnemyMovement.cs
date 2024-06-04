using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _screenBorder;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown; // time to change direction
    private float _wallDetectionCooldown; // time to change direction when colliding with wall
    private Camera _camera;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
        UpdateSpriteDirection();
    }

    private void UpdateTargetDirection() 
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
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

    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    private void SetVelocity() 
    {
        _rigidbody.velocity = _targetDirection.normalized * _speed;
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
            _targetDirection = -_targetDirection;

            _wallDetectionCooldown = 1f;
        }
    }
}
