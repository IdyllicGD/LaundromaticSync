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
        public NpcOrderTypes orderType;

        public bool isCleanOrder;
        public bool isDirtyOrder;
        public bool isRandomOrder;

        void Awake()
        {



            if (BasketType != BasketType.DryClean) return;
            //amountOfClothing = Random.Range(minAmount, maxAmount);
            amountOfClothing = 4;
            
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