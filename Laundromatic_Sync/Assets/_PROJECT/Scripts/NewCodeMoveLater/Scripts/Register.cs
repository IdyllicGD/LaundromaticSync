using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.temp.Scripts;
using FMODUnity;
using UnityEngine;

public class Register : MonoBehaviour, IMachineInteractable
{

    public void Interact(Camera camObject)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        if (!secondaryObject.TryGetComponent(out Basket basket)) return;
        if (basket.BasketType == BasketType.Folded)
        {
            Destroy(secondaryObject);
            //Add money
            //Play ding sound
        }
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
