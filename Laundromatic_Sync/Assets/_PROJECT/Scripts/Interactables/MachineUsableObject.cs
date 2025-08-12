using UnityEngine;

public class MachineUsableObjects : MonoBehaviour, IMachineUsableItems
{

    [SerializeField] MachineUsableItemsType machineUsableItemsType;
    
    public MachineUsableItemsType ReturnMachineUsableItemsType()
    {
        return machineUsableItemsType;
    }
}
