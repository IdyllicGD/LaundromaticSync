using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

public class PickupItem : MonoBehaviour, IPickupable
{
    Collider collider;
    Rigidbody rigidbody;
    public CleanOrDirty cleanOrDirty;
    //public MachineUsableItemsType machineUsableItemsType;

    private void Awake()
    {
        //machineUsableItemsType = MachineUsableItemsType.None;
        collider = GetComponent<Collider>();
        rigidbody.useGravity = false;
    }

    public void Pickup(Transform placePosition, bool isHoldingObject)
    {
        if (isHoldingObject) return;
        collider.enabled = false;
        transform.SetParent(placePosition);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }

    // public MachineUsableItemsType ReturnMachineUsableItemsType()
    // {
    //     return machineUsableItemsType;
    // }
}


