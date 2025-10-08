using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class HatPickUp : MonoBehaviour, Iinteractable
{
    public Transform hatHeadPosition;
    [SerializeField] PlayerInteraction playerInteraction;
    Collider collider;
    Rigidbody rb;

    void Start()
    {
        hatHeadPosition = playerInteraction.hatPosition;
        collider = GetComponent<Collider>();
    }
    public void Interact()
    {
        //Check if position is open
        if (playerInteraction.hatPosition.childCount < 0)
        {
            playerInteraction.UpdatePlayerPopupMessage("You already have a hat on");
            return;
        }
        rb.isKinematic = false;
        collider.enabled = false;
        transform.SetParent(hatHeadPosition);
        transform.DOLocalMove(Vector3.zero, 0.5f);
        transform.DOLocalRotate(Vector3.zero, 0.5f);
    }
}
