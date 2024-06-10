using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    public void OnCollected(GameObject player)
    {
        PlayerWeaponController weaponController = player.GetComponentInChildren<PlayerWeaponController>(); // Find the weapon controller in the player's children
        if (weaponController.IsWeaponInInventory(this.name)!= true)
        {
            weaponController.CollectWeapon(this.gameObject);
        }
    }

    public bool WeaponInInventory(PlayerWeaponController weaponController)
    {
        return weaponController.IsWeaponInInventory(this.name);
    }
}
