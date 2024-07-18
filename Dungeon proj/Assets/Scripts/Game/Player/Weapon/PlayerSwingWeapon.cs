using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwingWeapon : MonoBehaviour
{    
    [SerializeField]
    private int _damage;

    [SerializeField]
    private float _staminaCostPerShot;

    [SerializeField]
    private float _swingDuration;

    [SerializeField]
    private float _swingCooldown = 0.2f;

    [SerializeField]
    private Transform _weaponTransform;

    [SerializeField]
    private float _swingAngle;

    private bool _canSwing;
    public bool _isSwinging;
    private float _swingStartTime;
    private float _lastSwingTime;
    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private bool _swingBack;

    private CapsuleCollider2D _capsuleCollider;
    private StaminaController _staminaController;


    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (_capsuleCollider != null)
        {
            _capsuleCollider.enabled = false; // Ensure the collider is initially disabled
        }
        else
        {
            Debug.Log("Capsule collider not set!");
        }

        _staminaController = GetComponentInParent<StaminaController>();
    }

    private void Update()
    {
        if (!_isSwinging && _canSwing && _staminaController._currentStamina >= _staminaCostPerShot)
        {
            float timeSinceLastSwing = Time.time - _lastSwingTime;

            if (timeSinceLastSwing >= _swingCooldown)
            {
                StartSwing();
                
                _canSwing = false;
            }
        }

        if (_isSwinging)
        {
            _capsuleCollider.enabled = true;
            float elapsed = Time.time - _swingStartTime;
            float progress = elapsed / (_swingDuration / 2); // Split the swing duration into two phases

            if (!_swingBack)
            {
                if (progress >= 1f)
                {
                    _swingBack = true;
                    _swingStartTime = Time.time;
                }
                else
                {
                    _weaponTransform.localRotation = Quaternion.Lerp(_initialRotation, _targetRotation, progress);
                }
            }
            else
            {
                _weaponTransform.localRotation = Quaternion.Lerp(_targetRotation, _initialRotation, progress);
            }
        }
        else
        {
            _capsuleCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isSwinging && collision.CompareTag("Enemy"))
        {
            HealthController healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(_damage);

            FindObjectOfType<AudioManager>().PlaySFX("Bonk hit sound");
        }
    }

    public void StartSwing()
    {
        if (!_isSwinging)
        {
            _staminaController.ConsumeStamina(_staminaCostPerShot);

            _isSwinging = true;
            _swingStartTime = Time.time;
            _swingBack = false;
            _initialRotation = _weaponTransform.localRotation;
            if (_weaponTransform.localScale.y < 0)
            {
                _targetRotation = _initialRotation * Quaternion.Euler(0, 0, -_swingAngle);
            }
            else
            {
                _targetRotation = _initialRotation * Quaternion.Euler(0, 0, _swingAngle);
            }
            StartCoroutine(SwingCoroutine());
        }
    }

    private void EndSwing()
    {
        _isSwinging = false;
        _lastSwingTime = Time.time;
/*        _weaponTransform.localRotation = _initialRotation; // Reset rotation
        transform.localPosition = _initialPosition; // Reset position*/
    }

    private IEnumerator SwingCoroutine()
    {
 //       float halfDuration = _swingDuration / 2;
        while (Time.time - _swingStartTime < _swingDuration)
        {
            yield return null;
        }
        EndSwing();
    }

    private void OnFire(InputValue inputValue)
    {
        if (!PauseMenu.isPaused)
        {
            _canSwing = inputValue.isPressed;
        }
    }
}
