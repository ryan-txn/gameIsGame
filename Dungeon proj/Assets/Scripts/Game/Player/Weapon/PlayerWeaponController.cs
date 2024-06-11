using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    private GameObject[] weaponSlots = new GameObject[2]; //2 inventory slots array

    private Transform weaponParent;
    private int activeWeaponIndex = -1;

    private InputAction scrollAction;
    
    #region - Enable / Disable methods for scroll input -
    private void OnEnable()
    {
        if (scrollAction == null)
        {
            scrollAction = new InputAction("ScrollWheel", binding: "<Mouse>/scroll");
            scrollAction.performed += WeaponSwitch;
        }
        scrollAction.Enable();
    }

    private void OnDisable()
    {
        scrollAction.Disable();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        weaponParent = transform;
        
        // Initialize the first weapon (peastol) in inventory
        if (weaponParent.childCount > 0)
        {
            weaponSlots[0] = weaponParent.GetChild(0).gameObject;
            activeWeaponIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectWeapon(GameObject weaponObj)
    {
        WeaponIdentifier weaponIdentifier = weaponObj.GetComponent<WeaponIdentifier>();

        if (weaponIdentifier == null)
        {
            Debug.LogError("Weapon does not have a WeaponIdentifier component.");
            return;
        }

        int weaponIndex = weaponIdentifier.weaponIndex;

        // Find the weapon in the weaponParent
        Transform newWeaponTransform = weaponParent.GetChild(weaponIndex);
        
        if (newWeaponTransform == null)
        {
            Debug.LogError("Weapon not found in the weaponParent.");
            return;
        }

        GameObject newWeapon = newWeaponTransform.gameObject;
        
        // Check if the weapon is already in the inventory
        if (IsWeaponInInventory(weaponIndex))
        {
            Debug.LogError("Weapon already in inventory");
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
    }

    private void SwitchWeapon(int weaponIndex)
    {
        if (activeWeaponIndex != -1) //if there is weapon equipped
        {
            weaponSlots[activeWeaponIndex].SetActive(false);  //unequip current weapon
        }

        activeWeaponIndex = weaponIndex;
        weaponSlots[activeWeaponIndex].SetActive(true); // equip the new weapon
    }

    public bool IsWeaponInInventory(int weaponIndex)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null && weaponSlots[i].name == weaponParent.GetChild(weaponIndex).name)
            {
                return true;
            }
        }
        return false;
    }

    private int GetWeaponCount()
    {
        int count = 0;
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                count++;
            }
        }
        return count;
    }
    
    //player input weaponswitch on mouse scroll
    private void WeaponSwitch(InputAction.CallbackContext context)
    {
        // Only can weapon switch if you have more than 1 weapon in inventory
        if (!PauseMenu.isPaused && GetWeaponCount() > 1) 
        {
            Vector2 scrollValue = context.ReadValue<Vector2>();
            if (scrollValue.y != 0)
            {
                Debug.Log("Weapon changed by scrolling");
                int newIndex = (activeWeaponIndex + (scrollValue.y > 0 ? 1 : -1)) % weaponSlots.Length;
                if (newIndex < 0)
                {
                    newIndex = weaponSlots.Length - 1;
                }
                SwitchWeapon(newIndex);
            }
        }
    }
}
