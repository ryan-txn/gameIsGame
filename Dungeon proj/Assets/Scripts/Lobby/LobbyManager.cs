using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject player;

    private CoinController coinController;

    private void Awake()
    {
        coinController = player.GetComponent<CoinController>();
        Load();
    }

    public void Load()
    {
        DataManager.playerData = DataManager.saveSystem.LoadPlayer();
        Debug.Log("save file coins is " + DataManager.playerData.coins);
        coinController.ChangeCoinAmt(DataManager.playerData.coins);

        Debug.Log("loaded coins are " + coinController.coinAmt);
    }
}
