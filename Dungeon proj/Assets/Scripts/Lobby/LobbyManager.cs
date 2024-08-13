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
        // Check if the player data is loaded successfully
        if (DataManager.saveSystem.LoadPlayer() == null)
        {
            // If load fails, set default values and create a new save file
            Debug.Log("New SaveFile created");
            SetDefaultValues();
            Save();
        }
        else
        {
            // Proceed to apply loaded player data
            DataManager.playerData = DataManager.saveSystem.LoadPlayer();
            ApplyLoadedData();
        }
    }

    private void ApplyLoadedData()
    {
        // Apply loaded data to the game components
        coinController.ChangeCoinAmt(DataManager.playerData.coins);
        Debug.Log("Loaded coins: " + coinController.coinAmt);

        healthController.UpdateMaxHealth(DataManager.playerData.max_health);
        Debug.Log("Loaded max_health: " + healthController._maximumHealth);
        healthController.ResetHealth();

        staminaController.UpdateMaxStamina(DataManager.playerData.max_stamina);
        Debug.Log("Loaded max_stamina: " + staminaController._maximumStamina);
        staminaController.ResetStamina();

        playerMovement.UpdateSpeed(DataManager.playerData.speed);
        Debug.Log("Loaded speed: " + playerMovement.GetSpeed());
        playerAbility.UpdateCanUseAbility(DataManager.playerData.ability);
        Debug.Log("Loaded ability: " + playerAbility.CanUseAbility());

        //playerWeaponController.LoadWeaponSlots(DataManager.playerData.weapons);
    }

    private void SetDefaultValues()
    {
        // Set default values for a new save
        DataManager.playerData = new PlayerData
        {
            coins = 0,
            max_health = 100,
            curr_health = 100,
            max_stamina = 200,
            curr_stamina = 200,
            speed = 6,
            weapons = null, 
            ability = false
        };
        
        Debug.Log("Default player data has been set.");
    }

    public void Save()
    {
        Debug.Log("Game saved");
        DataManager.saveSystem.SavePlayer(coinController, staminaController, healthController, playerMovement, playerWeaponController, playerAbility);
    }
}
