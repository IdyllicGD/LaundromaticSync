using UnityEngine;

public interface IPickupable
{
    public void Pickup(Transform placePosition, float moveAmount, bool hasObjectInHand);

}

