using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaminaNumUI : MonoBehaviour
{
    [SerializeField]
    private bool _isUpgradeMenuText = false;

    private TMP_Text _staminaText;

    private void Awake()
    {
        _staminaText = GetComponent<TMP_Text>();
    }

    public void UpdateStaminaNum(StaminaController staminaController)
    {
        if (_isUpgradeMenuText == true)
        {
            _staminaText.text = $"MAX STAMINA: {staminaController.maxStaminaNum}"; //$ allows {} to be embedded within ""        
        }
        _staminaText.text = $"{staminaController.currentStaminaNum} /{staminaController.maxStaminaNum}"; //$ allows {} to be embedded within ""
    }
}
