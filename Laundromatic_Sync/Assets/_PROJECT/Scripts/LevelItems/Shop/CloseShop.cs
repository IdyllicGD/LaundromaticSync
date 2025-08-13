using _PROJECT._Jacques.Scripts.Interfaces;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class CloseShop : MonoBehaviour, Iinteractable
{
    public UnityEvent buttonPressed;
    bool isStoreOpen = true;
    [SerializeField] StudioEventEmitter buttonPressSoundOpen;
    [SerializeField] StudioEventEmitter buttonPressSoundClose;

    public void CloseStore()
    {
        if (isStoreOpen)
        {
            NpcManager.instance.canSpawnNpc = false;
            buttonPressSoundClose.Play();
            isStoreOpen = false;
        }
        else
        {
            NpcManager.instance.canSpawnNpc = true;
            buttonPressSoundOpen.Play();
            NpcManager.instance.SpawnNpc();
            isStoreOpen = true;
        }
    }

    public void Interact()
    {
        Debug.Log("[CloseShop] interactedWithButton");
        buttonPressed.Invoke();
    }
}
