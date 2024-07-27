using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicUiString : MonoBehaviour
{
    private TMP_Text selectedText;
    private AudioManager audioManager;

    void Start()
    {
        selectedText = GetComponent<TMP_Text>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMusicUIString()
    {
        selectedText.text = $"CURRENTLY SELECTED: \n {audioManager.GetCurrentList()}";
    }
}
