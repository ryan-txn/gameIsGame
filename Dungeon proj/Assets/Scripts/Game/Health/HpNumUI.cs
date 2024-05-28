using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpNumUI : MonoBehaviour
{
    private TMP_Text _healthText;

    private void Awake()
    {
        _healthText = GetComponent<TMP_Text>();
    }

    public void UpdateHealthNum(HealthController healthController)
    {
        _healthText.text = $"{healthController.currentHealthNum} /100"; //$ allows {} to be embedded within ""
    }
   
}
