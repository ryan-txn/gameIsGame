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
    private HealthController healthController;
    public CoinUI coinUI;

    private void Awake()
    {
        coinController = player.GetComponent<CoinController>();
        healthController = player.GetComponent<HealthController>();
    }


    public void OnPlayerDied()
    {
        Invoke(nameof(EndGame), _timetoWaitBeforeExit);
    }

    private void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(coinController, healthController);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        coinController.coinAmt = data.coins;
        coinUI.UpdateCoins(coinController);
        healthController._currentHealth = data.health;
        Debug.Log("loaded coins are " + coinController.coinAmt);
        Debug.Log("loaded health is " + healthController._currentHealth);
    }
}
