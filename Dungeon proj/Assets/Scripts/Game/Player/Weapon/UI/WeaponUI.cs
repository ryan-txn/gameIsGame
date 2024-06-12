using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField]
    public TMP_Text[] weaponSlotTexts;
    public Image[] weaponSlotImages;

    public Color activeWeaponColor = new Color(187, 149, 0, 1); // Highlight color
    public Color inactiveWeaponColor = Color.white; // Default color

    private void Awake()
    {
        // All weapon slot images are fully transparent initially
        foreach (var image in weaponSlotImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }

    public void UpdateWeaponUI(GameObject[] weaponSlots, int activeWeaponIndex)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                weaponSlotTexts[i].text = weaponSlots[i].name.ToUpper();
                weaponSlotImages[i].sprite = weaponSlots[i].GetComponent<SpriteRenderer>().sprite;
                weaponSlotImages[i].color = new Color(weaponSlotImages[i].color.r, weaponSlotImages[i].color.g, weaponSlotImages[i].color.b, 1); // Set to fully opaque
            }
            else
            {
                weaponSlotTexts[i].text = "-";
                weaponSlotImages[i].sprite = null;
                weaponSlotImages[i].color = new Color(weaponSlotImages[i].color.r, weaponSlotImages[i].color.g, weaponSlotImages[i].color.b, 0); // Set to fully transparent
            }

            // Highlight active weapon
            if (i == activeWeaponIndex)
            {
                weaponSlotTexts[i].color = activeWeaponColor;
            }
            else
            {
                weaponSlotTexts[i].color = inactiveWeaponColor;
            }
        }
    }
}
