using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class JamMeleeMovement : MonoBehaviour
{
    private bool _bossIsActivated = false;

    [SerializeField]
    private float _speed;

    //Variables for dashing
    [SerializeField]
    private float _dashSpeed;
    [SerializeField]
    private float _dashCooldown;
    [SerializeField]
    private float _dashDuration;
    [SerializeField]
    private float _chargeTime; // Time spent charging before dashing
    [SerializeField]
    private float _dashDirectionTimeLagFix = 0.3f; //Time gap for enemy tracking when charging, so player can escape the dash

    private ParticleSystem _chargeParticles; // Particle effect for charging
    [SerializeField]
    private TrailRenderer _trailRenderer; // Trail for dash
    [SerializeField]
    private GameObject _dashWarningArrow; // Arrow to indicate dash direction


    private bool _isIdle;
    private bool _isCharging;
    private bool _isDashing;

    private Vector2 _targetDirection;
    private Vector2 _dashDirection;

    private float _changeDirectionCooldown;
    private float _wallDetectionCooldown; // time to change direction when colliding with wall
    private float _idleTimer;
    private float _dashTimer;
    private float _dashCooldownTimer;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private HealthController _healthController;

    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up; // initial target direction will be the way it's currently facing
        _animator = GetComponentInChildren<Animator>();
        _healthController = GetComponent<HealthController>();

        // Make boss invincible before player enters room
        _healthController.IsInvincible = true;

        _chargeParticles = GetComponentInChildren<ParticleSystem>();

        _trailRenderer.enabled = false;
        _chargeParticles.Pause();

        if (_dashWarningArrow != null)
        {
            _dashWarningArrow.SetActive(false); // Initially hide the arrow
        }

        _dashCooldownTimer = _dashCooldown; // Initialize cooldown timer
    }

    private void FixedUpdate()
    {
        SetAnimation();
        if (_bossIsActivated)
        {
            //UpdateTargetDirection();
            if (_isIdle)
            {
                HandleIdleState();
            }
            else if (_isCharging)
            {
                HandleChargingState();
            }
            else if (_isDashing)
            {
                HandleDashingState();
            }
            else
            {
                SetVelocity();
                UpdateSpriteDirection();
                UpdateTargetDirection();
                HandleDashTime();
            }
        }
    }

    private void HandleIdleState()
    {
        _idleTimer -= Time.deltaTime;
        if (_idleTimer <= 0)
        {
            _isIdle = false;
            _dashCooldownTimer = _dashCooldown;
        }
    }

    private void HandleChargingState()
    {
        UpdateSpriteDirection();

        _dashTimer -= Time.deltaTime;
        if (_dashTimer >= _dashDirectionTimeLagFix)
        {
            _dashDirection = _playerAwarenessController.DirectionToPlayer.normalized; // make dash direction fixed by time lag before dash
        }

        if (_dashWarningArrow != null)
        {
            _dashWarningArrow.SetActive(true); // Show the arrow
                                               // Rotate to the dash direction

            _dashWarningArrow.transform.right = -_dashDirection;
        }

        if (_dashTimer <= 0)
        {
            _isCharging = false;
            StartDashing();
        }
    }

    private void HandleDashingState()
    {
        // Activate trail renderer during dashing
        if (!_trailRenderer.enabled)
        {
            _trailRenderer.enabled = true;
        }

        _dashTimer -= Time.deltaTime;
        if (_dashTimer <= 0)
        {
            StopDashing();
        }
    }

    private void StartCharging()
    {
        if (_chargeParticles != null)
        {
            _chargeParticles.Play();
        }

        _isCharging = true;
        _dashTimer = _chargeTime; // Set the charge timer

        _rigidbody.velocity = Vector2.zero;
        Debug.Log("Jam2 is now charging dash");
    }

    private void StartDashing()
    {
        // Disable the particle system during dashing
        if (_chargeParticles != null && _chargeParticles.isPlaying)
        {
            _chargeParticles.Stop();
        }
        _dashWarningArrow.SetActive(false);

        _isDashing = true;
        _dashTimer = _dashDuration; // Set dash duration, used by HandleDashDuration()

        _rigidbody.velocity = new Vector2(_dashDirection.x * _dashSpeed, _dashDirection.y * _dashSpeed);
        Debug.Log("Jam2 dashed");
    }

    private void StopDashing()
    {
        _isDashing = false;
        _idleTimer = 2f;
        _isIdle = true;

        _trailRenderer.enabled = false; // Disable trail renderer
        _rigidbody.velocity = Vector2.zero; // Stop movement
        Debug.Log("Jam2 is now idle");
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsIdle", _isIdle);
        _animator.SetBool("IsCharging", _isCharging);
    }


    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else if (!_isIdle && !_isDashing && !_isCharging) // Only check for random direction change if not already idling or dashing
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

    private void HandleDashTime()
    {
        if (_playerAwarenessController.AwareOfPlayer && !_isDashing && !_isCharging)
        {
            _dashCooldownTimer -= Time.deltaTime;
            if (_dashCooldownTimer <= 0)
            {
                StartCharging();
            }
        }
    }

    private void SetVelocity()
    {
        if (_isDashing)
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

    public void ActivateBoss()
    {
        _bossIsActivated = true;
        _healthController.IsInvincible = false; //Turns off boss invincibillity
        Debug.Log("Boss is activated");
    }
}
