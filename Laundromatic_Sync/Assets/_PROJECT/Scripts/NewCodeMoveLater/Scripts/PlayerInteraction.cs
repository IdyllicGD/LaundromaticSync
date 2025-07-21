using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform handPosition;
    private PlayerMovement playerMovement;
    InteractionType interactionType;

    #region Interaction Variables

    [SerializeField] float raycastDistance;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] LayerMask tableLayer;
    
    Camera playerCam;

    #endregion

    InputSystem_Actions inputActions;


    private void Start()
    {
        #region Input Actions

        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        inputActions.Player.Interact.performed += x => InteractWithObject();
        //TODO: Implement a way for the player to cancel the action
        //inputActions.Player.Back.performed += x => BackPressed();

        #endregion

        playerCam = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void InteractWithObject()
    {
        //Place object on Table
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitTable, raycastDistance,
                tableLayer))
        {
            if (handPosition.childCount == 0) return;
            
            handPosition.GetChild(0).GetComponent<Collider>().enabled = true;
            handPosition.GetChild(0).transform.DOMove(hitTable.point, .5f);
            handPosition.GetChild(0).SetParent(hitTable.transform);
        }

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit,
                raycastDistance,
                interactionLayer))
        {
            //pickup Interaction
            if (hit.transform.TryGetComponent(out IPickupable objectPickup))
            {
                objectPickup.Pickup(handPosition, handPosition.childCount != 0);
                return;
            }
        }
        

        //check if the player is holding an item
        if (handPosition.childCount == 0) return;
        GameObject itemInHand = handPosition.GetChild(0).gameObject;
        if (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit machineHit,
                raycastDistance,
                interactionLayer)) return;

        //Machine interaction
        if (machineHit.transform.TryGetComponent(out IMachineInteractable interactable))
        {
            interactable.Interact(playerCam, itemInHand);
            return;
        }

        //normal interaction
        if (machineHit.transform.TryGetComponent(out Iinteractable interactableObject))
        {
            interactableObject.Interact();
            return;
        }


    }

    public void StopPlayerMovement()
    {
        playerMovement.canMoveAndLook = false;
        playerMovement.inputActions.Disable();
    }

    public MachineType CheckMachineType(IMachineInteractable interactableMachine)
    {
        Debug.Log(interactableMachine.MachineType());
        return interactableMachine.MachineType();

    }

    void BackPressed()
    {
        if (!playerMovement.canMoveAndLook)
        {
            playerMovement.ResetCamera();
            playerMovement.canMoveAndLook = true;
        }

    }

    BasketType CheckBasketType(RaycastHit hit)
    {
        return hit.transform.TryGetComponent(out Basket basket)
            ? basket.ReturnBasketType()
            : BasketType.None;
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }
    
    
}

public enum InteractionType
{
    Pickup, Interact, Null
}
