using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _staminaBarForegroundImage;

    public void UpdateHealthBar(StaminaController staminaController)
    {
        _staminaBarForegroundImage.fillAmount = staminaController.RemainingStaminaPercentage;
    }
}
