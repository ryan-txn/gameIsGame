using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image itemImage;
    public Text itemNameText;
    public Text itemPriceText;

    private GameObject item;
    private int itemPrice;

    public void Setup(GameObject newItem)
    {
        item = newItem;
        itemPrice = 1; // to be changed

        itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
        itemNameText.text = item.name;
        itemPriceText.text = itemPrice.ToString() + "coins";
    }

    public void OnBuy()
    {
        
    }
}
