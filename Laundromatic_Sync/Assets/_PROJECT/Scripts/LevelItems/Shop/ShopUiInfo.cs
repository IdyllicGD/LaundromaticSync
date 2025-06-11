using System.Collections;
using UnityEngine;

public class ShopUiInfo : MonoBehaviour
{
    [SerializeField] GameObject laundryUpgrade;
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject shopRejectSound;
    private int laundryAmountAvailable = 1;
    
    public void UpgradeLaundry()
    {
        if (laundryAmountAvailable == 0 || PlayerManager.Instance.playerMoney < 30)
        {
            //playRejectSound
            shopRejectSound.SetActive(true);
            StartCoroutine(TimeToWait(1f));
            return;
        }

        if (PlayerManager.Instance.playerMoney >= 30)
        {
            PlayerManager.Instance.playerMoney -= 30;
            laundryUpgrade.SetActive(true);
            laundryAmountAvailable--;
            EndShop();
        }
        
    }

    public void EndShop()
    {
        shopUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PlayerManager.Instance.PlayerActionsEnableOrDisable(true);
    }

    IEnumerator TimeToWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        shopRejectSound.SetActive(false);
    }
}
