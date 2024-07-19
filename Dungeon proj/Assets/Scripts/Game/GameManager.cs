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

    private void Start()
    {
        if (DataManager.playerData == null)
        {
            Debug.Log("no player data file! (new save or lobby wasnt entered or logic bug)");
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
        DataManager.saveSystem.SavePlayer(coinController, staminaController, healthController, playerMovement);
    }

    public void OnLevelCleared()
    {
        DataManager.playerData.coins = coinController.coinAmt;
    }

    private void UpdatePlayerData()
    {
        //update coins in scene
        coinController = player.GetComponent<CoinController>();
        coinController.ChangeCoinAmt(DataManager.playerData.coins);
        Debug.Log("yokai!! coin controller coin amount is " + coinController.coinAmt);
        //update current and max health in scene
        healthController = player.GetComponent<HealthController>();
        healthController.UpdateCurrHealth(DataManager.playerData.curr_health);
        healthController.UpdateMaxHealth(DataManager.playerData.max_health);
        Debug.Log("yokai!! health controller current health is " + healthController._currentHealth + ", max health is " + healthController._maximumHealth);
        //update current and max stamina in scene
        staminaController = player.GetComponent<StaminaController>();
        staminaController.UpdateCurrStamina(DataManager.playerData.curr_stamina);
        staminaController.UpdateMaxStamina(DataManager.playerData.max_stamina);
        Debug.Log("yokai!! stamina controller current stamina is " + staminaController._currentStamina + ", max stamina is " + staminaController._maximumStamina);
        //update speed in scene
        playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.UpdateSpeed(DataManager.playerData.speed);
        Debug.Log("yokai!! player movement speed is " + playerMovement.GetSpeed());
    }
}
