using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _screenBorder;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    private Camera _camera;
    private Animator _animator; //for movement sprite

    private Vector2 _pointerInput;
    public Vector2 PointerInput => _pointerInput;
    [SerializeField]
    private InputActionReference pointerPosition;

    private WeaponParent _weaponParent;

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _weaponParent = GetComponentInChildren<WeaponParent>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
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
        SetPlayerVelocity();
        RotateTowardsMouse();
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

        PreventPlayerGoingOffScreen();
    }

    private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        //if x or y value is outside screen bounds, player has gone off screen
        //if player out of left/right bounds and wants to keep walking out
        if ((screenPosition.x < _screenBorder && _rigidbody.velocity.x < 0) ||
                (screenPosition.x > _camera.pixelWidth - _screenBorder && _rigidbody.velocity.x > 0))
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        }

        if ((screenPosition.y < _screenBorder && _rigidbody.velocity.y < 0) ||
                (screenPosition.y > _camera.pixelHeight - _screenBorder && _rigidbody.velocity.y > 0))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        }
    }

    private void RotateTowardsMouse() {
    // Get the mouse position in world coordinates
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 playerPosition = transform.position;

    // Determine the direction from the player to the mouse
    Vector2 directionToMouse = mousePosition - playerPosition;

    // Check if the mouse is to the right or left of the player
    if (directionToMouse.x > 0) {
        // Face right
        transform.rotation = Quaternion.Euler(0, 0, 0);
    } else if (directionToMouse.x < 0) {
        // Face left
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}

}
