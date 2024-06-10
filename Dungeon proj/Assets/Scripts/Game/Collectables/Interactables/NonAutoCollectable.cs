using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NonAutoCollectable : MonoBehaviour
{
    private interfaceCollectableBehaviour _collectableBehaviour;

    private bool _itemInteracted; 
    private WeaponCollectableBehaviour _weaponCollectableBehaviour;
    private PlayerWeaponController _playerWeaponController;

    private void Awake()
    {
        _collectableBehaviour = GetComponent<interfaceCollectableBehaviour>();
        _weaponCollectableBehaviour = GetComponent<WeaponCollectableBehaviour>();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        var player = collider.GetComponent<PlayerMovement>();
        _playerWeaponController = collider.GetComponentInChildren<PlayerWeaponController>();

        if (collider.gameObject.tag == "Player" && _itemInteracted)
        {
            if (_weaponCollectableBehaviour == null || !_playerWeaponController.IsWeaponInInventory(gameObject.name))
            {
                _collectableBehaviour.OnCollected(player.gameObject);
                Destroy(gameObject);
            }

            if(_playerWeaponController.IsWeaponInInventory(gameObject.name))
            {
                Debug.Log("Weapon already in inventory");
            }
        }
    }

    private void OnInteract(InputValue inputValue)
    {
        _itemInteracted = inputValue.isPressed;
    }
}
