using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class PickUpObject : MonoBehaviour
{
    [SerializeField] PickUpObjectType pickUpObjectType;
    public PickUpObjectType PickUpObjectType => pickUpObjectType;
    [Range(1,3)]
    private int laundryLoadAmount = 1;

    [SerializeField]private bool isFull = true;
    [SerializeField]public bool IsClean = false;

    private void Start()
    {
        // if (pickUpObjectType == PickUpObjectType.LaundryBasket)
        // {
        //     laundryLoadAmount = Random.Range(1, 3);
        //     UpdateLaundryBasketItems(laundryLoadAmount);
        // }
    }

    // private void Awake()
    // {
    //     if (pickUpObjectType == PickUpObjectType.LaundryBasket)
    //     {
    //         NpcManager.instance.basketID.Add(NpcManager.instance.npcCount, this.GameObject());
    //     }
    // }

    void UpdateLaundryBasketItems(int amount)
    {
        Debug.Log($"[Laundry Basket] load amount: {amount}");
    }
}

public enum PickUpObjectType
{
    LaundryBasket, Other
}
