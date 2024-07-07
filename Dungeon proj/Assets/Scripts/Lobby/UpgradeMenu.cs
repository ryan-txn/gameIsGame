using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject _upgradeMenu;
    public GameObject _player;

    private GameObject _weaponParent;

    private CoinController _coinController;
    private HealthController _healthController;
    private StaminaController _staminaController;

    private bool _menuInteracted;
    private bool _collided;
    private bool _menuIsOpen;

    void Start()
    {
        _upgradeMenu.SetActive(false);

        Transform weaponParentTransform = _player.transform.Find("WeaponParent"); 
        _weaponParent = weaponParentTransform.gameObject;

        _coinController = _player.GetComponent<CoinController>();
        _healthController = _player.GetComponent<HealthController>();
        _staminaController = _player.GetComponent<StaminaController>();
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
        int upgradeCost = 0; //10 coins
        float healthIncAmount = 10f; // increase by 10 health

        if (_coinController.coinAmt >= upgradeCost)
        {
            _coinController.DeductCoinAmt(upgradeCost);
            _healthController.AddMaxHealth(healthIncAmount);
            Debug.Log("Max health upgraded");
        }
    }

    public void UpgradeStamina()
    {
        int upgradeCost = 0; //10 coins
        float staminaIncAmount = 20f; // increase by 20 stamina

        if (_coinController.coinAmt >= upgradeCost)
        {
            _coinController.DeductCoinAmt(upgradeCost);
            _staminaController.AddMaxStamina(staminaIncAmount);
            Debug.Log("Max stamina upgraded");
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
