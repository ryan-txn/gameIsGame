using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField]
    private bool _abilityUnlocked = false;
    
    private bool _useAbility; // if input value is pressed
    private bool _canUseAbility = true; // if cooldown is 0

    public bool _isAbilityBought; //for player loading

    private float _cooldownTimer;
    private float _cooldownDuration;

    private PlayerMovement _playerMovement;

    [SerializeField]
    private AbilityBarUI _abilityBar;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _cooldownDuration = _playerMovement.DashCooldownDuration;
    }


    void Update()
    {
        if (_useAbility && _canUseAbility)
        {
            _playerMovement.TriggerDash();
            StartCoroutine(CooldownTimer());
            _useAbility = false;
        }
    }

    private IEnumerator CooldownTimer()
    {
        _canUseAbility = false;

        _cooldownTimer = _cooldownDuration + 0.25f; //offset

        while (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            _abilityBar.UpdateAbilityBar(_cooldownTimer / _cooldownDuration);
            yield return null;
        }

        _canUseAbility = true;
    }

    public void AbilityUnlocked()
    {
        _abilityUnlocked = true;
        _abilityBar.ShowAbilityUI();
    }

    private void OnAbility(InputValue inputValue)
    {
        if (!PauseMenu.isPaused && _abilityUnlocked && _canUseAbility)
        {
            _useAbility = inputValue.isPressed;
        }
    }

    public bool CanUseAbility()
    {
        return _isAbilityBought;
    }

    public void UpdateCanUseAbility(bool setAs)
    {
        _isAbilityBought = setAs;
        if (_isAbilityBought)
        {
            AbilityUnlocked();
        }
    }
}
