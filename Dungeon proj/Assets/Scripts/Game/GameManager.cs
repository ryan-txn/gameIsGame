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
        playerWeaponController = player.GetComponentInChildren<PlayerWeaponController>();
        playerAbility = player.GetComponent<PlayerAbility>();

        if (DataManager.playerData == null)
        {
            Debug.LogError("no player data file! (new save or lobby wasnt entered or logic bug)");
        }
        else
        {
            UpdatePlayerData();
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
        playerWeaponController.ClearWeaponSLots();
        int[] peastolSet = { 0 };
        playerWeaponController.LoadWeaponSlots(peastolSet);
        DataManager.saveSystem.SavePlayer(coinController, staminaController, healthController, playerMovement, playerWeaponController);
    }

    public void OnLevelCleared()
    {
        DataManager.playerData.coins = coinController.coinAmt;
    }

    private void UpdatePlayerData()
    {
        //update coins in scene
        coinController.ChangeCoinAmt(DataManager.playerData.coins);
        Debug.Log("yokai!! coin controller coin amount is " + coinController.coinAmt);
        //update current and max health in scene
        healthController.UpdateCurrHealth(DataManager.playerData.curr_health);
        healthController.UpdateMaxHealth(DataManager.playerData.max_health);
        Debug.Log("yokai!! health controller current health is " + healthController._currentHealth + ", max health is " + healthController._maximumHealth);
        //update current and max stamina in scene
        staminaController.UpdateCurrStamina(DataManager.playerData.curr_stamina);
        staminaController.UpdateMaxStamina(DataManager.playerData.max_stamina);
        Debug.Log("yokai!! stamina controller current stamina is " + staminaController._currentStamina + ", max stamina is " + staminaController._maximumStamina);
        //update speed and canuseability in scene
        playerMovement.UpdateSpeed(DataManager.playerData.speed);
        Debug.Log("yokai!! player movement speed is " + playerMovement.GetSpeed());
        playerMovement.UpdateCanUseAbility(DataManager.playerData.can_use_ability);
        playerAbility.UpdateCanUseAbility(DataManager.playerData.can_use_ability);
        Debug.Log("yokai!! player can use ability bool is " + playerMovement.CanUseAbility());

        //update weapons in scene
        playerWeaponController.LoadWeaponSlots(DataManager.playerData.weapons);
    }
}
