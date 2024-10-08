using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private Transform _gunOffset;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private float _staminaCostPerShot;

    private Transform _player;
    private StaminaController _staminaController;

    private bool _fireContinuously;
    private bool _fireSingle;
    private float _lastFireTime;


    void Awake() 
    {
        _player = GetComponentInParent<Transform>();
        _staminaController = GetComponentInParent<StaminaController>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((_fireContinuously || _fireSingle) && _staminaController._currentStamina >= _staminaCostPerShot)
        {
            float timeSinceLastFire = Time.time - _lastFireTime;
            
            if (timeSinceLastFire >= _timeBetweenShots)
            {
                FireBullet();
                _staminaController.ConsumeStamina(_staminaCostPerShot);
                _lastFireTime = Time.time;
                _fireSingle = false; //lets you single fire again by turning off delay expiry
            }
        }
    }

    private void FireBullet()
    {
        //create prefab of bullet at position of player
        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, _player.rotation);

        // Calculate direction from gun to mouse pointer
        Vector2 gunPosition = _gunOffset.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - gunPosition).normalized;

        // Set bullet's velocity in the direction of the calculated direction
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * _bulletSpeed;

        // Rotate the bullet to face the direction of travel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 270;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
