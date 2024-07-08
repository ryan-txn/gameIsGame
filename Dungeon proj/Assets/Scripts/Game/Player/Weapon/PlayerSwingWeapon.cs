using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwingWeapon : MonoBehaviour
{    
    [SerializeField]
    private int _damage;

    [SerializeField]
    private float _swingDuration;

    [SerializeField]
    private Transform _weaponTransform;

    [SerializeField]
    private float _swingAngle;

    public bool _isSwinging;
    private float _swingStartTime;
    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private Vector3 _initialPosition;
    private bool _swingBack;

    private CapsuleCollider2D _capsuleCollider;


    private void Awake()
    {
        _initialRotation = _weaponTransform.localRotation;
        _targetRotation = _initialRotation * Quaternion.Euler(0, 0, _swingAngle);
        _initialPosition = transform.localPosition;
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (_capsuleCollider != null)
        {
            _capsuleCollider.enabled = false; // Ensure the collider is initially disabled
        }
        else
        {
            Debug.Log("Capsule collider not set!");
        }
    }

    private void Update()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isSwinging && collision.collider.GetComponent<EnemyMovement>())
        {
            HealthController healthController = collision.collider.GetComponent<HealthController>();
            healthController.TakeDamage(_damage);
        }
    }

    public void StartSwing()
    {
        if (!_isSwinging)
        {
            _isSwinging = true;
            _swingStartTime = Time.time;
            _swingBack = false;
            StartCoroutine(SwingCoroutine());
        }
    }

    private void EndSwing()
    {
        _isSwinging = false;
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
            if (inputValue.isPressed)
            {
                StartSwing();
            }
        }
    }
}
