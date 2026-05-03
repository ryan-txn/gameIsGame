using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        if (!ControlMenu.isActive)
        {
        SceneManager.LoadScene("Lobby");

        }
    }

    public void Exit()
    {
        if (!ControlMenu.isActive)
        {
        // On WebGL (itch.io/browser) quit does nothing — redirect instead.
    #if UNITY_WEBGL && !UNITY_EDITOR
        Application.OpenURL("https://bungxd.itch.io"); // replace with your page URL
    #else
        Application.Quit();
    #endif
        }
    }

    public void ResetSave()
    {
        DataManager.saveSystem.ResetSave();
    }
}
