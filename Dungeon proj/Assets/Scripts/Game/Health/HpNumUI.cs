using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpNumUI : MonoBehaviour
{
    public bool _isUpgradeMenuText;
    private TMP_Text _healthText;

    private void Awake()
    {
        _healthText = GetComponent<TMP_Text>();
    }

    public void UpdateHealthNum(HealthController healthController)
    {
        if (_isUpgradeMenuText)
        {
            _healthText.text = $"MAX HEALTH: {healthController.maxHealthNum}";
        }
        _healthText.text = $"{healthController.currentHealthNum} /{healthController.maxHealthNum}"; //$ allows {} to be embedded within ""
    }
   
}
