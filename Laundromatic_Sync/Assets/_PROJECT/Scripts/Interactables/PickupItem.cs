using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Rigidbody))]
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
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    public void Pickup(Transform placePosition, bool isHoldingObject)
    {
        if (isHoldingObject) return;
        collider.enabled = false;
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        transform.SetParent(placePosition);
        transform.DOLocalMove(Vector3.zero, 0.2f);
        transform.DOLocalRotate(Vector3.zero, 0.2f);
        rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}


