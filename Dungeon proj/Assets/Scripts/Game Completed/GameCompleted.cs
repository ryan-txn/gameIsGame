using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCompleted : MonoBehaviour
{
    public void Retry()
    {
        //DataManager.saveSystem.ResetSave();
        SceneManager.LoadScene("MainMenu");
    }
}
