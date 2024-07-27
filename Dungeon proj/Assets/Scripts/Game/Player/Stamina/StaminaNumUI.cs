using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaminaNumUI : MonoBehaviour
{
    private TMP_Text _staminaText;

    public void UpdateStaminaNum(StaminaController staminaController)
    {
        _staminaText = GetComponent<TMP_Text>();
        _staminaText.text = $"{staminaController.CurrentStaminaNum} / {staminaController._maximumStamina}"; //$ allows {} to be embedded within ""
    }

    public void UpdateUpgradeMenu(StaminaController staminaController)
    {
        _staminaText = GetComponent<TMP_Text>();
        _staminaText.text = $"MAX STAMINA: {staminaController._maximumStamina}"; //$ allows {} to be embedded within ""
    }
}
