using UnityEngine;

public interface IPickupable
{
    public void Pickup(Transform placePosition, bool isHoldingObject);
}
