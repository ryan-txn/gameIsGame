using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBarUI : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image _abilityBarForegroundImage;

    [SerializeField]
    private UnityEngine.UI.Image _abilityBarBackgroundImage;

    [SerializeField]
    private UnityEngine.UI.Image _abilityImage;

    void Start()
    {
        // Hide the ability UI until it is unlocked
        _abilityBarBackgroundImage.fillAmount = 0f; 
        _abilityImage.fillAmount = 0f; 
    }

    public void UpdateAbilityBar(float fillAmount)
    {
        _abilityBarForegroundImage.fillAmount = fillAmount;
    }

    public void ShowAbilityUI()
    {
        // Show the ability UI when the ability is unlocked
        _abilityBarBackgroundImage.fillAmount = 1f; 
        _abilityImage.fillAmount = 1f; 
    }
}
