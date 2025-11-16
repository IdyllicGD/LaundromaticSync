using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.temp.Scripts;
using UnityEngine;

public class TrashCan : MonoBehaviour, IMachineInteractable
{

    public void Interact(Camera camObject)
    {
        // throw new System.NotImplementedException();
    }

    public void Interact(Camera camObject, GameObject secondaryObject, bool i)
    {
        if (secondaryObject.TryGetComponent(out DontDestroyObject dontDestroy)) return;
        Destroy(secondaryObject.gameObject);
    }

    public bool IsBusy()
    {
        throw new System.NotImplementedException();
    }

    public MachineType MachineType()
    {
        throw new System.NotImplementedException();
    }
}
