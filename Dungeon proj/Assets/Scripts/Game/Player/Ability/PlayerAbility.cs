using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbility : MonoBehaviour
{
    private bool _useAbility;
    private bool _canUseAbility = true;

    private Rigidbody2D _rigidbody;
    private TrailRenderer _trailRenderer;
    private InvincibilityController _invincibilityController;
    private PlayerMovement _playerMovement;

    [SerializeField]
    private float dashPower = 20f;
    [SerializeField]
    private float dashDuration = 0.5f;
    [SerializeField]
    private float dashCooldown = 2f;

    [SerializeField]
    private float invincibilityDuration = 0.5f;
    [SerializeField]
    private Color flashColor = Color.white;
    [SerializeField]
    private int numberOfFlashes = 3; // Number of flashes during invincibility

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _invincibilityController = GetComponent<InvincibilityController>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (_useAbility && _canUseAbility)
        {
            StartCoroutine(Dash());
            _useAbility = false;
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dash started");

        _canUseAbility = false;

        Vector2 originalVelocity = _rigidbody.velocity;

        _playerMovement.DisableMovement();

        Vector2 dashDirection = _playerMovement.MovementInput.normalized;
        Debug.Log("Dash direction: " + dashDirection);

        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(_playerMovement.DirectionToMouse.x, _playerMovement.DirectionToMouse.y);
            dashDirection.Normalize();
            Debug.Log("Dash direction defaulted to mousedirection: " + dashDirection);
        }

        _rigidbody.velocity = new Vector2(dashDirection.x * dashPower, dashDirection.y * dashPower);
        Debug.Log("Rigidbody velocity set to: " + _rigidbody.velocity);

        _invincibilityController.StartInvincibility(invincibilityDuration, flashColor, numberOfFlashes);
        _trailRenderer.emitting = true;        

        yield return new WaitForSeconds(dashDuration);

        _rigidbody.velocity = originalVelocity; // Reset velocity after dash
        _trailRenderer.emitting = false;
        Debug.Log("Dash ended");

        _playerMovement.EnableMovement();

        yield return new WaitForSeconds(dashCooldown);
        _canUseAbility = true;
        Debug.Log("Dash cooldown ended");
    }


    private void OnAbility(InputValue inputValue)
    {
        if (!PauseMenu.isPaused)
        {
            _useAbility = inputValue.isPressed;
        }
    }
}
