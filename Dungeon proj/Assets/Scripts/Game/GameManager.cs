using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _timetoWaitBeforeExit;


    public GameObject player;
    private CoinController coinController;

    private void Awake()
    {
        coinController = player.GetComponent<CoinController>();
    }


    public void OnPlayerDied()
    {
        Invoke(nameof(EndGame), _timetoWaitBeforeExit);
    }

    private void EndGame()
    {
        Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void Save()
    {
        SaveSystem.SavePlayer(coinController);
    }
}
