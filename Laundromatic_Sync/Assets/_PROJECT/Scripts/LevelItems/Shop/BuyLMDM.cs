using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BuyLMDM : MonoBehaviour
{
    [SerializeField] List<GameObject> laundryMachines = new();
    [SerializeField] List<GameObject> dryingMachines = new();
    [SerializeField] PlayerInteraction playerInteraction;
    int lmsBought;
    int dmsBought;
    int maxLm;
    int maxDm;

    void Start()
    {
        maxLm = laundryMachines.Count;
        maxDm = dryingMachines.Count;
    }
    public void SpawnInMachine(ItemsThatCanBeOrdered itemOrdered, int price)
    {
        if (itemOrdered == ItemsThatCanBeOrdered.Detergent || itemOrdered == ItemsThatCanBeOrdered.FabricSoftener) return;
        switch (itemOrdered)
        {
            case ItemsThatCanBeOrdered.LaundryMachine:
                if (lmsBought >= maxLm)
                {
                    playerInteraction.UpdatePlayerPopupMessage("You cannot buy more, you already have the max amount");
                    break;
                }
                else
                {
                    PlayerSettings.instance.ReduceMoney(price);
                    laundryMachines[lmsBought].SetActive(true);
                    lmsBought++;
                }
                break;
            case ItemsThatCanBeOrdered.DryerMachine:
                if (dmsBought>= maxDm)
                {
                    playerInteraction.UpdatePlayerPopupMessage("You cannot buy more, you already have the max amount");
                    break;
                }
                else
                {
                    dryingMachines[dmsBought].SetActive(true);
                    PlayerSettings.instance.ReduceMoney(price);
                    dmsBought++;
                }
                break;
        }
    }
}
