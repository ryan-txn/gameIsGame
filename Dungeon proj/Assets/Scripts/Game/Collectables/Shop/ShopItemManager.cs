using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemManager : MonoBehaviour
{
    public List<GameObject> availableItems;
    public Transform _shopItemContainer; //shop items instantiate here
    public int numberOfItemsInShop = 5;
    public Vector2 itemSpawnOffset = new Vector2(1, 0); // Offset for item positions


    private List<GameObject> currentShopItems = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        ClearShop();

        for (int i = 0; i < numberOfItemsInShop; i++)
        {
            GameObject randomItem = GetRandomItemFromPool();
            Vector2 spawnPosition = (Vector2)_shopItemContainer.position + itemSpawnOffset * i; // Spread items
            GameObject shopItem = Instantiate(randomItem, spawnPosition, Quaternion.identity, _shopItemContainer);
            
            NonAutoCollectable collectableComponent = shopItem.GetComponent<NonAutoCollectable>();
            if (collectableComponent != null)
            {
                collectableComponent._isInShop = true;
            }
            currentShopItems.Add(shopItem);
        }
    }

    private GameObject GetRandomItemFromPool()
    {
        int randomIndex = Random.Range(0, availableItems.Count);
        return availableItems[randomIndex];
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
