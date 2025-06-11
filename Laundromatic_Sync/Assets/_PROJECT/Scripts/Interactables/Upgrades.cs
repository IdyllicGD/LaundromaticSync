using _PROJECT._Jacques.Scripts.Interfaces;
using UnityEngine;

public class Upgrades : MonoBehaviour,Iinteractable 
{
    [SerializeField] GameObject upgradeUI;
    public void Interact()
    {
        upgradeUI.SetActive(true);
        PlayerManager.Instance.PlayerActionsEnableOrDisable(false);
        Cursor.lockState = CursorLockMode.None;
    }
}
