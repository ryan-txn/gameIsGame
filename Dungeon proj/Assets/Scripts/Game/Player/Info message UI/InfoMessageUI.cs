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

    public void UpdateMessage(string message, float fadeInDuration, float displayDuration, float fadeOutDuration)
    {
        _scoreText = GetComponent<TMP_Text>();
        //_scoreText.text = message;
        StartCoroutine(ShowMessage(message, fadeInDuration, displayDuration, fadeOutDuration));

    }

    public void ClearMessage()
    {
        if (_scoreText == null)
        {
            _scoreText = GetComponent<TMP_Text>();
        }
        _scoreText.text = string.Empty;
    }

    private IEnumerator ShowMessage(string message, float fadeInDuration, float displayDuration, float fadeOutDuration)
    {
        _scoreText.text = message;
        yield return StartCoroutine(FadeTextToFullAlpha(fadeInDuration));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeTextToZeroAlpha(fadeOutDuration));
    }

    private IEnumerator FadeTextToFullAlpha(float duration)
    {
        _scoreText.color = new Color(_scoreText.color.r, _scoreText.color.g, _scoreText.color.b, 0);
        while (_scoreText.color.a < 1.0f)
        {
            _scoreText.color = new Color(_scoreText.color.r, _scoreText.color.g, _scoreText.color.b, _scoreText.color.a + (Time.deltaTime / duration));
            yield return null;
        }
    }

    private IEnumerator FadeTextToZeroAlpha(float duration)
    {
        _scoreText.color = new Color(_scoreText.color.r, _scoreText.color.g, _scoreText.color.b, 1);
        while (_scoreText.color.a > 0.0f)
        {
            _scoreText.color = new Color(_scoreText.color.r, _scoreText.color.g, _scoreText.color.b, _scoreText.color.a - (Time.deltaTime / duration));
            yield return null;
        }
        _scoreText.text = string.Empty;
    }
}
