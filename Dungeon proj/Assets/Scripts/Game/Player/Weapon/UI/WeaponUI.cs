using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public TMP_Text[] weaponSlotTexts;
    public Image[] weaponSlotImages;
    private PlayerWeaponController _playerWeaponController;

    private void Awake()
    {
        _playerWeaponController = GetComponent<PlayerWeaponController>();
        weaponSlotTexts = new TMP_Text[_playerWeaponController.inventorySize];
        weaponSlotImages = new Image[_playerWeaponController.inventorySize];

    }

    public void UpdateWeaponUI(GameObject[] weaponSlots, int activeWeaponIndex)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                weaponSlotTexts[i].text = weaponSlots[i].name;
                weaponSlotImages[i].sprite = weaponSlots[i].GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                weaponSlotTexts[i].text = "Empty";
                weaponSlotImages[i].sprite = null;
            }
        }

    }
}
