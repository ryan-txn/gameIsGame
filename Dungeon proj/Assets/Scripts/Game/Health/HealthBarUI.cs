using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _healthBarForegroundImage;

    [SerializeField]
    private UnityEngine.UI.Image _healthBarBackgroundImage;

    [SerializeField]
    private TMP_Text _text;

    public void UpdateHealthBar(HealthController healthController)
    {
        _healthBarForegroundImage.fillAmount = healthController.RemainingHealthPercentage;
    }

    // Deactivate boss health bar UI
    public void DeactivateHealthBar()
    {
        _healthBarForegroundImage.gameObject.SetActive(false);
        _healthBarBackgroundImage.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
    }

    // Activate boss health bar UI
    public void ActivateHealthBar()
    {
        _healthBarForegroundImage.gameObject.SetActive(true);
        _healthBarBackgroundImage.gameObject.SetActive(true);
        _text.gameObject.SetActive(true);
    }
}
