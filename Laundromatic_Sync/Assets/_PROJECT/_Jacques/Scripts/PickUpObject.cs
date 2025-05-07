using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class PickUpObject : MonoBehaviour
{
    [SerializeField] PickUpObjectType pickUpObjectType;
    public PickUpObjectType PickUpObjectType => pickUpObjectType;
    [Range(1,3)]
    private int laundryLoadAmount;

    private void Start()
    {
        if (pickUpObjectType == PickUpObjectType.LaundryBasket)
        {
            laundryLoadAmount = Random.Range(1, 3);
            UpdateLaundryBasketItems(laundryLoadAmount);
        }
    }

    void UpdateLaundryBasketItems(int amount)
    {
        Debug.Log($"[Laundry Basket] load amount: {amount}");
    }
}

public enum PickUpObjectType
{
    LaundryBasket, Other
}
