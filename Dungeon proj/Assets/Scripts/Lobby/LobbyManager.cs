using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject player;

    private CoinController coinController;
    private HealthController healthController;
    private StaminaController staminaController;
    private PlayerMovement playerMovement;
    private PlayerWeaponController playerWeaponController;
    private PlayerAbility playerAbility;

    private void Start()
    {
        coinController = player.GetComponent<CoinController>();
        healthController = player.GetComponent<HealthController>();
        staminaController = player.GetComponent<StaminaController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAbility = player.GetComponent<PlayerAbility>();
        playerWeaponController = player.GetComponentInChildren<PlayerWeaponController>();
        Load();
    }

    public void Load()
    {
        DataManager.playerData = DataManager.saveSystem.LoadPlayer();

        coinController.ChangeCoinAmt(DataManager.playerData.coins);
        Debug.Log("loaded coins are " + coinController.coinAmt);

        healthController.UpdateMaxHealth(DataManager.playerData.max_health);
        Debug.Log("loaded max_health is " + healthController._maximumHealth);
        healthController.ResetHealth();

        staminaController.UpdateMaxStamina(DataManager.playerData.max_stamina);
        Debug.Log("loaded max_stamina is " + staminaController._maximumStamina);
        staminaController.ResetStamina();

        playerMovement.UpdateSpeed(DataManager.playerData.speed);
        Debug.Log("loaded speed is " + playerMovement.GetSpeed());
        playerAbility.UpdateCanUseAbility(DataManager.playerData.ability);
        Debug.Log("loaded ability bool is " + playerAbility.CanUseAbility());

        //playerWeaponController.LoadWeaponSlots(DataManager.playerData.weapons);
    }

    public void Save()
    {
        Debug.Log("Game saved");
        DataManager.saveSystem.SavePlayer(coinController, staminaController, healthController, playerMovement, playerWeaponController, playerAbility);
    }
}
