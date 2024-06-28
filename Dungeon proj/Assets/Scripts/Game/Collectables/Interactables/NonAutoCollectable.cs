using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NonAutoCollectable : MonoBehaviour
{
    private interfaceCollectableBehaviour _collectableBehaviour;
    private WeaponCollectableBehaviour _weaponCollectableBehaviour;
    private PlayerWeaponController _playerWeaponController;
    private CoinController _coinController;
    private ShopAreaDetection _shopAreaDetection;

    private bool _itemInteracted;
    public int itemPrice;
    public bool _isInShop;

    private void Awake()
    {
        _collectableBehaviour = GetComponent<interfaceCollectableBehaviour>();
        _weaponCollectableBehaviour = GetComponent<WeaponCollectableBehaviour>();
        _shopAreaDetection = FindObjectOfType<ShopAreaDetection>();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        var player = collider.GetComponent<PlayerMovement>();
        _playerWeaponController = collider.GetComponentInChildren<PlayerWeaponController>();
        _coinController = collider.GetComponent<CoinController>();

        if (collider.gameObject.tag == "Player")
        {
            if (_isInShop && _shopAreaDetection != null)
            {
                //show shop merchant the item price
                _shopAreaDetection.ShowItemPrice(itemPrice);
            }

            if (_itemInteracted)
            {
                if (_isInShop)
                {
                    if (_coinController != null && _coinController.coinAmt >= itemPrice)
                    {
                        if (_weaponCollectableBehaviour == null || !_playerWeaponController.IsWeaponInInventory(GetComponent<WeaponIdentifier>().weaponIndex))
                        {
                            _coinController.DeductCoinAmt(itemPrice);
                            _collectableBehaviour.OnCollected(player.gameObject);
                            Destroy(gameObject);
                        }

                        if (_weaponCollectableBehaviour != null && _playerWeaponController.IsWeaponInInventory(GetComponent<WeaponIdentifier>().weaponIndex))
                        {
                            Debug.Log("Weapon already in inventory");
                        }
                    }
                    else
                    {
                        Debug.Log("Not enough coins to buy this item.");
                    }
                }
                else
                {
                    // Free item on map
                    if (_weaponCollectableBehaviour == null || !_playerWeaponController.IsWeaponInInventory(GetComponent<WeaponIdentifier>().weaponIndex))
                    {
                        _collectableBehaviour.OnCollected(player.gameObject);
                        Destroy(gameObject);
                    }

                    if (_weaponCollectableBehaviour != null && _playerWeaponController.IsWeaponInInventory(GetComponent<WeaponIdentifier>().weaponIndex))
                    {
                        Debug.Log("Weapon already in inventory");
                    }
                }
            }
        }
    }

    private void OnInteract(InputValue inputValue)
    {
        _itemInteracted = inputValue.isPressed;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && _isInShop && _shopAreaDetection != null)
        {
            _shopAreaDetection.HideItemPrice();
        }
    }
}
