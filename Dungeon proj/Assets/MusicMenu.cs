using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMenu : MonoBehaviour
{
    public GameObject musicMenu;
    public GameObject uiPanel;
    public static bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isActive)
            {
                ResumeGame();
            }
            else
            {
                LoadMenu();
            } 
        }
    }

    public void LoadMenu()
    {
        musicMenu.SetActive(true);
        uiPanel.SetActive(false);
        isActive = true;
        Debug.Log("Music panel set active");
    }

    public void ResumeGame()
    {
        musicMenu.SetActive(false);
        uiPanel.SetActive(true);
        isActive = false;
        Debug.Log("Music panel set inactive");
    }

    public void ChooseSWPlaylist()
    {
        FindObjectOfType<AudioManager>().SelectMusicList1();
    }

    public void ChooseRyanPlaylist()
    {
        FindObjectOfType<AudioManager>().SelectMusicList2();
    }
}
