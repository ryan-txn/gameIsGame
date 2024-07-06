using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    public float _currentHealth;
    [SerializeField]
    private float _maximumHealth;
    public float RemainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    public float currentHealthNum
    {
        get {return _currentHealth;}
    }

    public float maxHealthNum
    {
        get {return _maximumHealth;}
    }

    //invincibility frames
    public bool IsInvincible { get; set; }

    public UnityEvent OnDied;

    public UnityEvent OnDamaged; //for invincibility effect

    public UnityEvent OnHealthChanged; //want to invoke when health is added or removed, then reflect on health bar UI

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0) 
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }

        _currentHealth -= damageAmount;

        OnHealthChanged.Invoke();


        if (_currentHealth < 0) 
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamaged.Invoke();
        }
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth) {
            return;
        }

        _currentHealth += amountToAdd;

        if (_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth;
        }

        OnHealthChanged.Invoke();
    }
}
