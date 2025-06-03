using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public Dictionary<string, GameObject> ShopItems;
    
    private void Awake()
    {
        Instance = this;
    }
    
    //Run when trying to buy an item from the store
    void BuyItem(string itemName)
    {
        if (PlayerManager.Instance.playerMoney >= ShopItems[itemName].gameObject.GetComponent<ItemInfo>().price)
        {
            PlayerManager.Instance.playerMoney -= ShopItems[itemName].gameObject.GetComponent<ItemInfo>().price;
            PlayerManager.Instance.playerInventory.Add(itemName, ShopItems[itemName].gameObject);
        }
    }
    
    
    
    
}
