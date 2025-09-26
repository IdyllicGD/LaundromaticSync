using _PROJECT._Jacques.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace _PROJECT.Scripts.temp.Scripts
{
    [RequireComponent(typeof(Renderer))]
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
        public NpcID npcID;
        public ParticleSystem dirtySystem;
        public ParticleSystem cleanSystem;
        public Color wetCleanColour;
        public Color dryCleanColour;
        public Color foldedCleanColour;
        public InteractionNameData interactionNameData;
        [SerializeField] Image imageToChange;
        Renderer rend;

        void Awake()
        {

            rend = GetComponent<Renderer>();
            npcID = transform.parent.parent.GetComponent<NpcOrderData>().npcID;
            imageToChange.sprite = transform.parent.parent.GetComponent<NpcOrderData>().characterImage; 
            amountOfClothing = 4;
            NpcManager.instance.currentBaskets.Add(gameObject);
            dirtySystem.Play();
            

        }

        public BasketType ReturnBasketType()
        {
            return BasketType;
        }

        public MachineUsableItemsType ReturnMachineUsableItemsType()
        {
            return usableItemsType;
        }

        public void UpdateBasketInfoWet()
        {
            BasketType = BasketType.WetClean;
            dirtySystem.Stop();
            cleanSystem.Play();
            gameObject.SetActive(true);
            gameObject.GetComponent<Collider>().enabled = true;
            rend.material.color = wetCleanColour;
            interactionNameData.itemName = $"Clean Wet Basket";
        }

        public void UpdateBasketInfoDry()
        {
            BasketType = BasketType.DryClean;
            rend.material.color = dryCleanColour;
            interactionNameData.itemName = $"Clean Dry Basket Unfolded";
            gameObject.GetComponent<Collider>().enabled = true;
        }
        public void UpdateBasketInfoFolded()
        {
            BasketType = BasketType.Folded;
            interactionNameData.itemName = $"Clean Folded Basket";
            gameObject.GetComponent<Collider>().enabled = true;
            rend.material.color = foldedCleanColour;
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