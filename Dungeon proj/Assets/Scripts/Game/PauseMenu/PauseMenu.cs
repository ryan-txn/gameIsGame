using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public UpgradeMenu upgradeMenu;
    public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            } 
        }
        
    }

    public void PauseGame()
    {
        FindObjectOfType<AudioManager>().PlaySFX("Pause sfx");

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        FindObjectOfType<AudioManager>().PlaySFX("Unpause sfx");

        pauseMenu.SetActive(false);
        if (upgradeMenu == null)
        {
            Time.timeScale = 1f;
        }
        else if (!upgradeMenu.menuIsOpen)
        {
            Time.timeScale = 1f;
        }
        isPaused = false;
    }

    public void GotoMainMenu()
    {
        FindObjectOfType<AudioManager>().PlaySFX("Button click");

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        isPaused = false;
    }
}
