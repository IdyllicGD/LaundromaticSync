using _PROJECT._Jacques.Scripts.Interfaces;
using UnityEngine;

namespace _PROJECT.Scripts.temp.Scripts
{
    public class Basket : MonoBehaviour, IMachineUsableItems
    {

        public BasketType BasketType;
        public ItemsUsed ItemsUsed;
        public MachineUsableItemsType usableItemsType;
        public int amountOfClothing;
        public int maxAmount;
        public int minAmount;

        void Awake()
        {
            if (BasketType != BasketType.DryClean) return;
            amountOfClothing = Random.Range(minAmount, maxAmount);
        }

        public BasketType ReturnBasketType()
        {
            return BasketType;
        }

        public MachineUsableItemsType ReturnMachineUsableItemsType()
        {
            return usableItemsType;
        }
    }
    public enum BasketType
    {
        Dirty, WetClean, DryClean, Folded, None
    }
    public enum ItemsUsed
    {
        FabricSoftner, None
    }
}