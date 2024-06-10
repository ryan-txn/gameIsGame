using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private GameObject[] weaponSlots = new GameObject[4]; //4 inventory slots

    private Transform weaponParent;

    private int activeWeaponIndex = -1;


    // Start is called before the first frame update
    void Start()
    {
        weaponParent = GetComponent<Transform>();
        
        // Initialize the first weapon (peastol) in inventory and set as active if not already active
        if (weaponParent.childCount > 0)
        {
            weaponSlots[0] = weaponParent.GetChild(0).gameObject;
            activeWeaponIndex = 0;
            weaponSlots[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectWeapon(GameObject weaponObj)
    {
        // Find the weapon in the weaponParent
        Transform newWeaponTransform = weaponParent.Find(weaponObj.name);
        
        if (newWeaponTransform == null)
        {
            Debug.LogError("Weapon not found in the weaponParent.");
            return;
        }

        GameObject newWeapon = newWeaponTransform.gameObject;

        // Check if the weapon is already in the inventory
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null && weaponSlots[i].name == newWeapon.name)
            {
                // If the weapon exists in the inventory, switch to it
                SwitchWeapon(i);
                return;
            }
        }

        // Add new weapon into first available slot
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                weaponSlots[i] = newWeapon;
                SwitchWeapon(i);
            }
        }
        Debug.Log("Inventory is full. Cannot collect new weapon.");
    }

    private void SwitchWeapon(int weaponIndex)
    {
        if (activeWeaponIndex != 1) //if there is weapon equipped
        {
            weaponSlots[activeWeaponIndex].SetActive(false);  //unequip current weapon
        }

        activeWeaponIndex = weaponIndex;
        weaponSlots[activeWeaponIndex].SetActive(true); // equip the new weapon
    }
}
