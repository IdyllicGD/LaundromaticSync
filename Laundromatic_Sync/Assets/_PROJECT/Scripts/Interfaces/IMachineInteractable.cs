using UnityEngine;
using UnityEngine.InputSystem;

namespace _PROJECT._Jacques.Scripts.Interfaces
{
    public interface IMachineInteractable
    {
        public MachineType MachineType();
        public bool IsBusy();
        public void Interact(Camera camObject);
        public void Interact(Camera camObject, GameObject secondaryObject, bool wasThrown);
        
    }

    public enum MachineType
    {
        Laundry, Drying, Folding, Shop, Other
    }
}