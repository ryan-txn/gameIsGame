using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject player;

    private CoinController coinController;
    private HealthController healthController;
    private StaminaController staminaController;

    private void Start()
    {
        coinController = player.GetComponent<CoinController>();
        healthController = player.GetComponent<HealthController>();
        staminaController = player.GetComponent<StaminaController>();
        Load();
    }

    public void Load()
    {
        DataManager.playerData = DataManager.saveSystem.LoadPlayer();
        coinController.ChangeCoinAmt(DataManager.playerData.coins);
        healthController.UpdateMaxHealth(DataManager.playerData.max_health);
        healthController.ResetHealth();
        staminaController.UpdateMaxStamina(DataManager.playerData.max_stamina);
        staminaController.ResetStamina();


        Debug.Log("loaded coins are " + coinController.coinAmt);
        Debug.Log("loaded max_health is " + healthController._maximumHealth);
        Debug.Log("loaded max_stamina is " + staminaController._maximumStamina);

    }
}
