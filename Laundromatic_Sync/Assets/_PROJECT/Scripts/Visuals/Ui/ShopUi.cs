using UnityEngine;

public class ShopUi : MonoBehaviour
{
    [SerializeField] GameObject boughtItem;
    [SerializeField] private float cost;
    bool ownItem = false;
    
    
    public void BuyItem()
    {
        if (ownItem)
        {
            Debug.Log("Already own the item");
            return;
        }
        if (PlayerManager.Instance.playerMoney >= cost)
        {
            PlayerManager.Instance.playerMoney =- cost;
            boughtItem.SetActive(true);
            ownItem = true;
        }
        else
        {
            Debug.Log("Broke Bitch");
        }
    }
}
