using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    private TMP_Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateScore(CoinController coinController)
    {
        _scoreText.text = $"COINS: {coinController.coinAmt}"; //$ allows {} to be embedded within ""
    }
    
}
