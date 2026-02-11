using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

public class HatPickUp : MonoBehaviour, Iinteractable
{
    public Transform hatHeadPosition;
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] StudioEventEmitter sparklySound;
    [SerializeField] StudioEventEmitter touchSound;
    Collider collider;
    Rigidbody rb;

    void Start()
    {
        hatHeadPosition = playerInteraction.hatPosition;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        touchSound.Play();
    }

    void OnCollisionExit(Collision collision)
    {
        touchSound.Stop();
    }
    public void Interact()
    {
        //Check if position is open
        if (playerInteraction.hatPosition.childCount < 0)
        {
            playerInteraction.UpdatePlayerPopupMessage("You already have a hat on");
            return;
        }
        sparklySound.Play();
        rb.isKinematic = true;
        rb.useGravity = false;
        collider.enabled = false;
        //Play sparkly sound
        transform.SetParent(hatHeadPosition);
        transform.DOLocalMove(Vector3.zero, 0.5f);
        transform.DOLocalRotate(Vector3.zero, 0.5f);
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }
}
