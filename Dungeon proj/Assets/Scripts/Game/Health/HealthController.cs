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
    public float _maximumHealth;
    public float RemainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    //invincibility frames
    public bool IsInvincible { get; set; }

    public UnityEvent OnDied;

    public UnityEvent OnDamaged; //for invincibility effect

    public UnityEvent OnHealthChanged; //want to invoke when health is added or removed, then reflect on health bar UI

    private GameObject bossRoom;

    [SerializeField]
    public string dieSfx;

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

    public void AddMaxHealth(float amountToAdd)
    {
        _maximumHealth += amountToAdd;
        _currentHealth += amountToAdd;
        OnHealthChanged.Invoke();
    }

    public void UpdateMaxHealth(float newMaxHealth)
    {
        _maximumHealth = newMaxHealth;
        OnHealthChanged.Invoke();
    }

    public void UpdateCurrHealth(float newCurrHealth)
    {
        _currentHealth = newCurrHealth;
        OnHealthChanged.Invoke();
    }

    public void DecreaseEnemyCount()
    {
        EnemyCounter.RemoveEnemy();
    }

    public void ResetHealth()
    {
        _currentHealth = _maximumHealth;
        OnHealthChanged.Invoke();
    }

    public void SpawnPortal()
    {
        SpawnPortal spawnPortal = FindObjectOfType<SpawnPortal>();
        if (spawnPortal != null)
        {
            GameObject Portal = spawnPortal.Spawn();
            SceneTransition sceneTransition = Portal.GetComponent<SceneTransition>();
            sceneTransition.UpdateSceneToLoadString();
        }
        else
        {
            Debug.Log("Spawn Portal not found - either not boss room or error");
        }
    }

    public void PlayDieSFX()
    {
        FindObjectOfType<AudioManager>().PlaySFX(dieSfx);
    }
}
