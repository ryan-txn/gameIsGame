using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]
    private int INVENTORY_SIZE;

    [SerializeField]
    private GameObject[] collectibleWeaponPrefabs; //To instantiate weapons when replacing them

    public int inventorySize
    {
        get { return INVENTORY_SIZE; }
        set
        {
            INVENTORY_SIZE = value;  // Update weaponSlots array size here }
        }
    }

    private GameObject[] weaponSlots;

    private Transform weaponParent;
    private int activeWeaponIndex = -1;

    public WeaponUI weaponUI;

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
        weaponSlots = new GameObject[INVENTORY_SIZE]; //2 Inventory slots array
        weaponParent = transform;

        // Initialize the active weapon and add it into weaponslot inventory
        for (int i = 0; i < weaponParent.childCount; i++)
        {
            if (weaponParent.GetChild(i).gameObject.activeSelf)
            {
                weaponSlots[0] = weaponParent.GetChild(i).gameObject; // Add the initially active weapon to the first slot
                activeWeaponIndex = 0; // Set the active weapon index
                break; // Exit loop after finding the initially active weapon
            }
        }
        
        weaponUI.UpdateWeaponUI(weaponSlots, activeWeaponIndex);
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
                Destroy(weaponObj); 
                return;
            }
        }

        // If inventory full, replace unique weapon
        if (GetWeaponCount() == weaponSlots.Length)
        {
            ReplaceWeapon(newWeapon, weaponIndex);
            Destroy(weaponObj); 
        }
        newWeapon.transform.SetParent(weaponParent);
        newWeapon.transform.localRotation = Quaternion.identity;
    }

    // Unequip current weapon and instantiate a weapon collectable
    private void ReplaceWeapon(GameObject newWeapon, int weaponIndex)
    {

        if (weaponSlots != null)
        {        
            int oldWeaponIndex = GetWeaponIndex(weaponSlots[activeWeaponIndex]);
            Instantiate(collectibleWeaponPrefabs[oldWeaponIndex], transform.position, Quaternion.identity);
            // Deactivate the current weapon
            weaponSlots[activeWeaponIndex].SetActive(false);
        }

        // Replace with the new weapon
        weaponSlots[activeWeaponIndex] = newWeapon;
        // Activate the new weapon
        weaponSlots[activeWeaponIndex].SetActive(true);

        weaponUI.UpdateWeaponUI(weaponSlots, activeWeaponIndex);
        FindObjectOfType<AudioManager>().PlaySFX("Weapon pickup");
    }

    private int GetWeaponIndex(GameObject weapon)
    {
        for (int i = 0; i < weaponParent.childCount; i++)
        {
            if (weaponParent.GetChild(i).gameObject == weapon)
            {
                return i;
            }
        }
        return -1; // Should never happen if weapons are managed correctly
    }

    private void SwitchWeapon(int weaponIndex)
    {
        if (activeWeaponIndex != -1) //if there is weapon equipped
        {
            weaponSlots[activeWeaponIndex].SetActive(false);  //unequip current weapon
        }

        activeWeaponIndex = weaponIndex;
        weaponSlots[activeWeaponIndex].SetActive(true); // equip the new weapon

        weaponUI.UpdateWeaponUI(weaponSlots, activeWeaponIndex);

        FindObjectOfType<AudioManager>().PlaySFX("Weapon pickup");
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
