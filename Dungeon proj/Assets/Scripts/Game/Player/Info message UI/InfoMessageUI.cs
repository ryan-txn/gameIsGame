using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoMessageUI : MonoBehaviour
{
    private TMP_Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
    }

    public void UpdateMessage(string message)
    {
        _scoreText = GetComponent<TMP_Text>();
        _scoreText.text = message; //$ allows {} to be embedded within ""
    }

    public void ClearMessage()
    {
        if (_scoreText == null)
        {
            _scoreText = GetComponent<TMP_Text>();
        }
        _scoreText.text = string.Empty;
    }
}
