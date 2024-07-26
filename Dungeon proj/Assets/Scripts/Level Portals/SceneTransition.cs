using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    public string _sceneToLoad;

    private bool _doorAccessed;
    private bool _collided;

    private CoinController coinController;
    private HealthController healthController;
    private StaminaController staminaController;
    private PlayerMovement playerMovement;
    private PlayerWeaponController playerWeaponController;
    private PlayerAbility playerAbility;

    private void Update()
    {
        if (_collided && _doorAccessed)
        {
            if (_sceneToLoad != "MainMenu" && _sceneToLoad != "Lobby")
            {
                FindObjectOfType<AudioManager>().PlaySFX("Portal sfx");
            }
            
            UpdatePlayerData();
            SceneManager.LoadScene( _sceneToLoad );
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.tag == "Player")
        {
            _collided = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            _collided = false;
        }
    }

    private void OnInteract(InputValue inputValue)
    {
        _doorAccessed = inputValue.isPressed;
    }

    private void UpdatePlayerData()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("erm.. Player not found... what the sigma...");
        }
        else
        {
            //update coins
            coinController = player.GetComponent<CoinController>();
            DataManager.playerData.coins = coinController.coinAmt;
            //update max and current health
            healthController = player.GetComponent<HealthController>();
            DataManager.playerData.curr_health = healthController._currentHealth;
            DataManager.playerData.max_health = healthController._maximumHealth;
            //update max and current stamina
            staminaController = player.GetComponent<StaminaController>();
            DataManager.playerData.curr_stamina = staminaController._currentStamina;
            DataManager.playerData.max_stamina = staminaController._maximumStamina;
            //update speed and can use ability
            playerMovement = player.GetComponent<PlayerMovement>();
            DataManager.playerData.speed = playerMovement.GetSpeed();
            playerAbility = player.GetComponent<PlayerAbility>();
            DataManager.playerData.ability = playerAbility.CanUseAbility();
            //update weapons
            playerWeaponController = player.GetComponentInChildren<PlayerWeaponController>();
            if (_sceneToLoad == "Level 1-1")
            {
                int[] peastolSet = { 0 };
                DataManager.playerData.weapons = peastolSet;
            }
            else
            {
                DataManager.playerData.weapons = playerWeaponController.GetInventoryIndexes();
            }
        }
    }

    public void UpdateSceneToLoadString()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("current build index " + currentSceneIndex);
        int nextSceneIndex = currentSceneIndex + 1;
        Debug.Log("next build index " + nextSceneIndex);

        string nextSceneName = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
        Debug.Log("next scene path " + nextSceneName);

        nextSceneName = System.IO.Path.GetFileNameWithoutExtension(nextSceneName);
        Debug.Log("loading in scene " + nextSceneName);

        //set the scene to load to the next scene
        _sceneToLoad = nextSceneName;
    }
}
