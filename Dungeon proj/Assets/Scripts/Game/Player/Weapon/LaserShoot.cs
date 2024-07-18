using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class LaserShoot : MonoBehaviour
{
    [SerializeField]
    private float defaultDistRay = 100f;

    [SerializeField]
    private Transform _gunOffset;

    public LineRenderer _lineRenderer;

    [SerializeField]
    private LayerMask _layerMask; // LayerMask to exclude certain layers like player

    Transform _gunTransform;
    StaminaController _staminaController;

    [SerializeField]
    private float _staminaCostPerShot;

    [SerializeField]
    private float _timeBetweenDamage;

    [SerializeField]
    private int laserDamage = 10; // Damage value for the laser

    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastDamageTime;

    [SerializeField]
    GameObject _hitParticleEffect;

    void Awake()
    {
        _hitParticleEffect.SetActive(false);
        _gunTransform = GetComponent<Transform>();
        _staminaController = GetComponentInParent<StaminaController>();
    }

    void Update()
    {
        if ((_fireContinuously || _fireSingle) && _staminaController._currentStamina >= _staminaCostPerShot)
        {

            ShootLaser();
            _staminaController.ConsumeStamina(_staminaCostPerShot);
            _fireSingle = false;
        }
        else
        {
            ClearLaser();
        }
    }

    void ShootLaser()
    {
        RaycastHit2D _hit = Physics2D.Raycast(_gunTransform.position, transform.right, defaultDistRay, ~_layerMask);
        if (_hit.collider != null)
        {
            HandleHit(_hit);
            Draw2DRay(_gunOffset.position, _hit.point);
        }
        else
        {
            //else ray will reach until default ray distance limit
            Draw2DRay(_gunOffset.position, _gunOffset.transform.right * defaultDistRay);
        }
    }

    void ClearLaser()
    {
        _hitParticleEffect.SetActive(false);
        _lineRenderer.positionCount = 0;
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, endPos);

        _hitParticleEffect.SetActive(true);
        _hitParticleEffect.transform.position = _lineRenderer.GetPosition(1); //hit particle effect set to end of laser
    }

    void HandleHit(RaycastHit2D hit)
    {
        HealthController healthController = hit.collider.GetComponent<HealthController>();
        if (healthController != null)
        {
            float _timeSinceLastHit = Time.time - _lastDamageTime;
            if (_timeSinceLastHit > _timeBetweenDamage)
            {
                healthController.TakeDamage(laserDamage);
                _lastDamageTime = Time.time;
            }
        }
    }

    private void OnFire(InputValue inputValue)
    {
        if (!PauseMenu.isPaused)
        {
            _fireContinuously = inputValue.isPressed;

            if (inputValue.isPressed)
            {
                _fireSingle = true;
            }
        }
    }
}
