using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    public float playerSpeedStat
    {
        get {return _speed;}
    }

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;

    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    private Animator _animator; //for movement sprite

    private Vector2 _pointerInput;
    public Vector2 PointerInput => _pointerInput;
    private Vector2 directionToMouse;

    [SerializeField]
    private InputActionReference pointerPosition;

    private WeaponParent _weaponParent;

    //Dash Ability variables
    private TrailRenderer _trailRenderer;
    private InvincibilityController _invincibilityController;
    private bool _isDashing;
    private bool _canUseAbility = true;

    [SerializeField]
    private float dashPower = 28f;
    [SerializeField]
    private float dashDuration = 0.5f;
    [SerializeField]
    private float dashCooldown = 1f;

    public float DashCooldownDuration => dashCooldown + dashDuration;

    [SerializeField]
    private float invincibilityDuration = 0.5f;
    [SerializeField]
    private Color flashColor = Color.white;
    [SerializeField]
    private int numberOfFlashes = 1; // Number of flashes during invincibility

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _weaponParent = GetComponentInChildren<WeaponParent>();
        _animator = GetComponent<Animator>();

        //Dash components
        _invincibilityController = GetComponent<InvincibilityController>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        _pointerInput = GetPointerInput();
        _weaponParent.PointerPosition = _pointerInput;
    }

    //unity's FixedUpdate() method. called at the frequency of the physics system
    //any changes to a rigidbody shld be made here
    private void FixedUpdate()
    {
        if (!_isDashing)
        {
            SetPlayerVelocity();
            RotateTowardsMouse();
        }
        SetAnimation();
    }

    private void OnMove(InputValue inputValue) {
        _movementInput = inputValue.Get<Vector2>();
    }

    public Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void SetAnimation() 
    {
        bool isMoving = _movementInput != Vector2.zero;
        _animator.SetBool("IsMoving", isMoving); //calls IsMoving paramter that is set in Animator Parameters
    }

    private void SetPlayerVelocity()
    {
        _smoothedMovementInput = Vector2.SmoothDamp(
                    _smoothedMovementInput,
                    _movementInput,
                    ref _movementInputSmoothVelocity, 0.1f  //time for transition to take, 0.1s
                );

        _rigidbody.velocity = _smoothedMovementInput * _speed;

    }

    private void RotateTowardsMouse()
    {
        // Get the mouse position in world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;

        // Determine the direction from the player to the mouse
        directionToMouse = mousePosition - playerPosition;

        // Check if the mouse is to the right or left of the player
        if (directionToMouse.x > 0)
        {
            // Face right
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (directionToMouse.x < 0)
        {
            // Face left
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void IncreaseSpeed(float amountToAdd)
    {
        _speed += amountToAdd;
    }

    public void UpdateSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void TriggerDash()
    {
        if (_canUseAbility)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dash started");

        _canUseAbility = false;
        _isDashing = true;


        Vector2 dashDirection = _movementInput.normalized;
        Debug.Log("Dash direction: " + dashDirection);

        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(directionToMouse.x, directionToMouse.y);
            dashDirection.Normalize();
            Debug.Log("Dash direction defaulted to mousedirection: " + dashDirection);
        }

        _rigidbody.velocity = new Vector2(dashDirection.x * dashPower, dashDirection.y * dashPower);
        Debug.Log("Rigidbody velocity set to: " + _rigidbody.velocity);

        _invincibilityController.StartInvincibility(invincibilityDuration, flashColor, numberOfFlashes);
        _trailRenderer.emitting = true;        

        yield return new WaitForSeconds(dashDuration);

        SetPlayerVelocity(); // Reset velocity after dash
        _trailRenderer.emitting = false;
        _isDashing = false;
        Debug.Log("Dash ended");

        yield return new WaitForSeconds(dashCooldown);
        _canUseAbility = true;
        Debug.Log("Dash cooldown ended");
    }

}
