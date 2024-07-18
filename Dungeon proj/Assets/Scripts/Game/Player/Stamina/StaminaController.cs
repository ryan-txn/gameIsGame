using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaminaController : MonoBehaviour
{
    [SerializeField]
    public float _currentStamina;

    [SerializeField]
    public float _maximumStamina;

    public float RemainingStaminaPercentage
    {
        get
        {
            return _currentStamina / _maximumStamina;
        }
    }

    public float CurrentStaminaNum
    {
        get {return (float)Math.Round(_currentStamina, 1); }
    }



    public UnityEvent OnStaminaChanged;

    public void ConsumeStamina(float staminaAmount) 
    {
        //return if no stamina or not enough stamina
        if (_currentStamina == 0 || staminaAmount > _currentStamina) 
        {
            return;
        }

        _currentStamina -= staminaAmount;
        
        OnStaminaChanged.Invoke();
    }

    public void RecoverStamina(float staminaAmount)
    {
        if (_currentStamina == _maximumStamina)
        {
            return;
        }

        _currentStamina += staminaAmount;

        if (_currentStamina > _maximumStamina)
        {
            _currentStamina = _maximumStamina;
        }

        OnStaminaChanged.Invoke();
    }

    public void AddMaxStamina(float amountToAdd)
    {
        _maximumStamina += amountToAdd;
        _currentStamina += amountToAdd;
        OnStaminaChanged.Invoke();
    }

    public void UpdateMaxStamina(float newMaxStamina)
    {
        _maximumStamina = newMaxStamina;
        OnStaminaChanged.Invoke();
    }

    public void UpdateCurrStamina(float newCurrStamina)
    {
        _currentStamina = newCurrStamina;
        OnStaminaChanged.Invoke();
    }

    public void ResetStamina()
    {
        _currentStamina = _maximumStamina;
        OnStaminaChanged.Invoke();
    }
}
