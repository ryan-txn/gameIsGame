using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    [SerializeField]
    public GameObject weaponPrefab;


    public void OnCollected(GameObject player)
    {
        PlayerWeaponController weaponController = player.GetComponentInChildren<PlayerWeaponController>(); // Find the weapon controller in the player's children
        if (weaponController != null)
        {
            weaponController.CollectWeapon(weaponPrefab);
        }
    }
}
