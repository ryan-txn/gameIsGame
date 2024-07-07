using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaminaNumUI : MonoBehaviour
{
    private TMP_Text _staminaText;

    private void Awake()
    {
        _staminaText = GetComponent<TMP_Text>();
    }

    public void UpdateHealthNum(StaminaController staminaController)
    {
        _staminaText.text = $"{staminaController.currentStaminaNum} /{staminaController.maxStaminaNum}"; //$ allows {} to be embedded within ""
    }
}
