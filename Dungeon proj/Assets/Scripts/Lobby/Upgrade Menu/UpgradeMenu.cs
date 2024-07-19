using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject _upgradeMenu;
    public GameObject _player;

    private GameObject _weaponParent;

    private CoinController _coinController;
    private HealthController _healthController;
    private StaminaController _staminaController;
    private PlayerMovement _playerMovementScript;
    private PlayerAbility _playerAbilityScript;


    private bool _menuInteracted;
    private bool _collided;
    private bool _menuIsOpen;
    public bool menuIsOpen
    {
        get { return _menuIsOpen; }
    }

    //Upgrade costs and amounts
    [SerializeField]
    private int upgradeCost = 0; //for hp, sp, speed
    [SerializeField]
    private float healthIncAmount = 10f;
    [SerializeField]
    float staminaIncAmount = 20f;
    [SerializeField]
    float speedIncAmount = 1f;

    [SerializeField]
    private int abilityUnlockCost = 0;

    //Upgrade values
    private float originalHealthNum = 100f;
    private float originalStaminaNum = 200f;
    private float originalSpeedNum = 6f;

    private float potentialHealthNum;
    private float potentialStaminaNum;
    private float potentialSpeedNum;


    // Upgrade limits and countdown counters
    private int healthUpgradeLimit = 3;
    private int staminaUpgradeLimit = 3;
    private int speedUpgradeLimit = 3;

    private int healthUpgradeCount = 0;
    private int staminaUpgradeCount = 0;
    private int speedUpgradeCount = 0;

    private bool abilityUnlocked = false;

    // Upgrade buttons
    public Button healthUpgradeButton;
    public Button staminaUpgradeButton;
    public Button speedUpgradeButton;
    public Button abilityUnlockButton;

    private TMP_Text healthUpgradeButtonText;
    private TMP_Text staminaUpgradeButtonText;
    private TMP_Text speedUpgradeButtonText;
    private TMP_Text abilityUnlockButtonText;

    [SerializeField]
    private TMP_Text abilityDescriptionText;

    void Start()
    {
        _upgradeMenu.SetActive(false);

        Transform weaponParentTransform = _player.transform.Find("WeaponParent");
        _weaponParent = weaponParentTransform.gameObject;

        _coinController = _player.GetComponent<CoinController>();
        _healthController = _player.GetComponent<HealthController>();
        _staminaController = _player.GetComponent<StaminaController>();
        _playerMovementScript = _player.GetComponent<PlayerMovement>();
        _playerAbilityScript = _player.GetComponent<PlayerAbility>();

        healthUpgradeButtonText = healthUpgradeButton.GetComponentInChildren<TMP_Text>();
        staminaUpgradeButtonText = staminaUpgradeButton.GetComponentInChildren<TMP_Text>();
        speedUpgradeButtonText = speedUpgradeButton.GetComponentInChildren<TMP_Text>();
        abilityUnlockButtonText = abilityUnlockButton.GetComponentInChildren<TMP_Text>();

    }

    private void InitialiseCounter()
    {
        potentialHealthNum = originalHealthNum + healthUpgradeLimit * healthIncAmount;
        potentialStaminaNum = originalStaminaNum + staminaUpgradeLimit * staminaIncAmount;
        potentialSpeedNum = originalSpeedNum + speedUpgradeLimit * speedIncAmount;

        healthUpgradeCount = (int) ((potentialHealthNum - _healthController._currentHealth) / healthIncAmount);
        staminaUpgradeCount = (int) ((potentialStaminaNum - _staminaController.CurrentStaminaNum) / staminaIncAmount);
        speedUpgradeCount =  (int) ((potentialSpeedNum - _playerMovementScript.playerSpeedStat) / speedIncAmount);

        Debug.Log("health upgrade countdown:" + healthUpgradeCount);
        Debug.Log("stamina upgrade countdown:" + staminaUpgradeCount);
        Debug.Log("speed upgrade countdown:" + speedUpgradeCount);
    }

    void Update()
    {
        if (_collided && _menuInteracted)
        {
            if (!_menuIsOpen)
            {
                OpenMenu();
            }
        }
    }

    public void UpgradeHealth()
    {
        if (_coinController.coinAmt >= upgradeCost && healthUpgradeCount > 0)
        {
            _coinController.DeductCoinAmt(upgradeCost);
            _healthController.AddMaxHealth(healthIncAmount);
            Debug.Log("Max health upgraded");

            InitialiseCounter();
            UpdateButtonStates();
        }
    }

    public void UpgradeStamina()
    {
        if (_coinController.coinAmt >= upgradeCost && staminaUpgradeCount > 0)
        {
            _coinController.DeductCoinAmt(upgradeCost);
            _staminaController.AddMaxStamina(staminaIncAmount);
            Debug.Log("Max stamina upgraded");

            staminaUpgradeCount++;
            InitialiseCounter();
            UpdateButtonStates();
        }
    }

    public void UpgradeSpeed()
    {
        if (_coinController.coinAmt >= upgradeCost && speedUpgradeCount > 0)
        {
            _coinController.DeductCoinAmt(upgradeCost);
            _playerMovementScript.IncreaseSpeed(speedIncAmount);
            Debug.Log("Max speed upgraded");

            speedUpgradeCount++;
            InitialiseCounter();
            UpdateButtonStates();
        }
    }

    public void UnlockAbility()
    {
        if (_coinController.coinAmt >= abilityUnlockCost && !abilityUnlocked)
        {
            _coinController.DeductCoinAmt(abilityUnlockCost);

            Debug.Log("Ability unlocked");

            abilityUnlocked = true;
            _playerAbilityScript.AbilityUnlocked();
            UpdateButtonStates();
        }
    }

    private void UpdateButtonStates()
    {
        // Disable buttons if limits are reached

        //HEALTH
        if (healthUpgradeCount <= 0)
        {
            healthUpgradeButtonText.text = "MAXED";
            healthUpgradeButton.interactable = false;
        }

        //STAMINA
        if (staminaUpgradeCount <= 0)
        {
            staminaUpgradeButtonText.text = "MAXED";
            staminaUpgradeButton.interactable = false;
        }

        //SPEED
        if (speedUpgradeCount <= 0)
        {
            speedUpgradeButtonText.text = "MAXED";
            speedUpgradeButton.interactable = false;
        }

        //ABILITY
        if (healthUpgradeCount + staminaUpgradeCount + speedUpgradeCount <= 4)
        {
            abilityDescriptionText.color = Color.white;
            abilityUnlockButton.interactable = true;
        }

        if (abilityUnlocked)
        {
            abilityUnlockButtonText.text = "UNLOCKED";
            abilityUnlockButton.interactable = false;
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

    public void OpenMenu()
    {
        InitialiseCounter();
        UpdateButtonStates();

        _upgradeMenu.SetActive(true);
        Time.timeScale = 0f; //pauses background
        _menuIsOpen = true;
        _weaponParent.SetActive(false);
    }

    public void CloseMenu()
    {
        _upgradeMenu.SetActive(false);
        Time.timeScale = 1f;
        _menuIsOpen = false;
        _weaponParent.SetActive(true);
    }

    private void OnInteract(InputValue inputValue)
    {
        _menuInteracted = inputValue.isPressed;
    }
}
