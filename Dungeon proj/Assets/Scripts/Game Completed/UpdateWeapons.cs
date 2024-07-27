using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWeapons : MonoBehaviour
{
    public WeaponUI weaponUI;
    private GameObject[] weaponSlots;
    private int INVENTORY_SIZE = 2;
    private Transform weaponParent;


    // Start is called before the first frame update
    void Start()
    {
        weaponSlots = new GameObject[INVENTORY_SIZE];
        UpdateObjectList(DataManager.playerData.weapons);
    }

    private void UpdateObjectList(int[] weaponIndexes)
    {
        weaponSlots = new GameObject[INVENTORY_SIZE];
        for (int i = 0; i < weaponIndexes.Length; i++)
        {
            if (weaponIndexes[i] != -1)
            {
                int weaponIndex = weaponIndexes[i];
                weaponParent = transform;
                Transform weaponTransform = weaponParent.GetChild(weaponIndex);
                GameObject weapon = weaponTransform.gameObject;
                weaponSlots[i] = weapon;
            }
            else
            {
                weaponSlots[i] = null; // No weapon in this slot
            }
        }
        weaponUI.inactiveWeaponColor = Color.black;
        weaponUI.UpdateWeaponUI(weaponSlots, -1);
    }
}
