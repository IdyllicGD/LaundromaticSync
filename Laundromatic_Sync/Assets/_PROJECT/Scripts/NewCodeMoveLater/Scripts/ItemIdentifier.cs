using UnityEngine;
public class ItemIdentifier : MonoBehaviour, IMachineUsableItems
{
        [SerializeField]MachineUsableItemsType machineUsableItemsType;

        public MachineUsableItemsType ReturnMachineUsableItemsType()
        {
                return machineUsableItemsType;
        }
}
