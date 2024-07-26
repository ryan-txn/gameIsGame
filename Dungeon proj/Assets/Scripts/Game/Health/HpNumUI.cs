using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpNumUI : MonoBehaviour
{
    private TMP_Text _healthText;

    public void UpdateHealthNum(HealthController healthController)
    {
        _healthText = GetComponent<TMP_Text>();
        _healthText.text = $"{healthController._currentHealth} /{healthController._maximumHealth}"; //$ allows {} to be embedded within ""
    }

    public void UpdateUpgradeMenu(HealthController healthController)
    {
        _healthText = GetComponent<TMP_Text>();
        _healthText.text = $"MAX HEALTH: {healthController._maximumHealth}";        
    }
   
}
