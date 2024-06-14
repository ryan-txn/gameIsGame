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
        PlayerData data = SaveSystem.LoadPlayer();
        Debug.Log("save file coins is " + data.coins);
        coinController.ChangeCoinAmt(data.coins);

        Debug.Log("loaded coins are " + coinController.coinAmt);
    }
}
