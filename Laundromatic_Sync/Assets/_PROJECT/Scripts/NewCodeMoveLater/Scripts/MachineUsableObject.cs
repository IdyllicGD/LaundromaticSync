using UnityEngine;

public class Detergent : MonoBehaviour, IMachineUsableItems
{

    [SerializeField] MachineUsableItemsType machineUsableItemsType;
    public MachineUsableItemsType ReturnMachineUsableItemsType()
    {
        return machineUsableItemsType;
    }
}
