using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;

public class PickupItem : MonoBehaviour, IPickupable
{
    Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    public void Pickup(Transform placePosition, bool isHoldingObject)
    {
        if (isHoldingObject) return;
        collider.enabled = false;
        transform.SetParent(placePosition);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }
}
