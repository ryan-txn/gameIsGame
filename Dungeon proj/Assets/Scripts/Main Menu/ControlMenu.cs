using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    public GameObject controlMenu;
    public static bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        controlMenu.SetActive(false); 
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
        controlMenu.SetActive(true);
        isActive = true;
    }

    public void ResumeGame()
    {
        controlMenu.SetActive(false);
        isActive = false;
    }
}
