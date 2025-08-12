using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Title("Ordered Items", null, TitleAlignments.Centered)]
    public List<GameObject> orderList = new();
    [Title("Items That Can Be Ordered", null, TitleAlignments.Centered)]
    [SerializeField] GameObject laundryMachine;
    [SerializeField] GameObject dryerMachine;
    [SerializeField] GameObject detergent;
    [SerializeField] GameObject softener;

    void Start()
    {
        instance = this;

    }

    public void OrderItem(ItemsThatCanBeOrdered itemOrdered)
    {
        switch (itemOrdered)
        {
            case ItemsThatCanBeOrdered.LaundryMachine:
                orderList.Add(laundryMachine);
                break;
            case ItemsThatCanBeOrdered.DryerMachine:
                orderList.Add(dryerMachine);
                break;
            case ItemsThatCanBeOrdered.Detergent:
                orderList.Add(detergent);
                break;
            case ItemsThatCanBeOrdered.FabricSoftener:
                orderList.Add(softener);
                break;
            case ItemsThatCanBeOrdered.Other:
                Debug.Log($"Invalid thing ordered, How tf did you order this?");
                break;
        }
    }
}

public enum ItemsThatCanBeOrdered
{
    LaundryMachine, DryerMachine, Detergent, FabricSoftener, Other
}
