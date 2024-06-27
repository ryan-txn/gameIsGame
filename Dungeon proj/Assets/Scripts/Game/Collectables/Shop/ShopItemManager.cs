using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemManager : MonoBehaviour
{
    public List<GameObject> playerItems;
    public List<GameObject> weaponItems;
    public Transform _shopItemContainer; //shop items instantiate here
    public int numberOfItemsInShop = 4;
    public Vector2 itemSpawnOffset = new Vector2(2, 0); // Offset for item positions


    private List<GameObject> currentShopItems = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        ClearShop();

        List<GameObject> selectedPlayerItems = GetRandomUniqueItem(playerItems, 2);
        List<GameObject> selectedWeaponItems = GetRandomUniqueItem(weaponItems, 2);

        List<GameObject> allSelectedItems = new List<GameObject>();
        allSelectedItems.AddRange(selectedPlayerItems);
        allSelectedItems.AddRange(selectedWeaponItems);

        for (int i = 0; i < allSelectedItems.Count; i++)
        {
            Vector2 spawnPosition = (Vector2)_shopItemContainer.position + itemSpawnOffset * i; 
            GameObject shopItem = Instantiate(allSelectedItems[i], spawnPosition, Quaternion.identity, _shopItemContainer);

            NonAutoCollectable collectableComponent = shopItem.GetComponent<NonAutoCollectable>();
            if (collectableComponent != null)
            {
                collectableComponent._isInShop = true;
            }
            currentShopItems.Add(shopItem);
        }

    }

    private List<GameObject> GetRandomUniqueItem(List<GameObject> itemList, int num)
    {
        if (itemList.Count == num)
        {
            return itemList;
        }

        List<GameObject> tempList = new List<GameObject>(itemList);
        List<GameObject> selectedItemList = new List<GameObject>();

        int index = itemList.Count;
        for (int i = 0; i < num; i++)
        {
            int randomIndex = Random.Range(0, index);
            selectedItemList.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
            index -= 1;
        }
        return selectedItemList;
    }

    private void ClearShop()
    {
        foreach (GameObject item in currentShopItems)
        {
            Destroy(item);
        }
        currentShopItems.Clear();
    }
}
