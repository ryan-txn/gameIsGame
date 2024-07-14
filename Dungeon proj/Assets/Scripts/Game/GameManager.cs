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
        if (DataManager.playerData == null)
        {
            Debug.Log("no save file");
        } 
        else
        {
            coinController.ChangeCoinAmt(DataManager.playerData.coins);
        }
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
        DataManager.saveSystem.SavePlayer(coinController);
    }
}
