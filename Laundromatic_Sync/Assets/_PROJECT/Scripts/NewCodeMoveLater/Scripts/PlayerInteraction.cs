using System;
using System.Collections;
using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Behavior;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Title("Player", null, TitleAlignments.Centered)]
    [SerializeField] Transform handPosition;
    private PlayerMovement playerMovement;
    private InteractionType interactionType;

    #region Interaction Variables

    [Title("Interaction Vars", null, TitleAlignments.Centered)]
    [Range(0f, 10f)]
    [SerializeField] float raycastDistance;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] LayerMask tableLayer;
    [SerializeField] LayerMask npcLayer;
    [SerializeField] float dialogueCooldownTimer;
    
    Camera playerCam;

    #endregion

    InputSystem_Actions inputActions;
    bool canIncreaseDialogue = true;


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
        //Interact With NPC
        if (canIncreaseDialogue && Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitNPC, raycastDistance, npcLayer))
        {
            canIncreaseDialogue = false;
            if (!hitNPC.transform.TryGetComponent(out BehaviorGraphAgent npcBehaviour)) return;
            npcBehaviour.BlackboardReference.GetVariable("canTalk", out BlackboardVariable<bool> _canTalk);
            if (!_canTalk) return;
            Debug.Log("NpcHit");
            npcBehaviour.BlackboardReference.GetVariable("isDoneTalking", out BlackboardVariable<bool> _isDoneTalking);

            #region NPC END CODE

            if (handPosition.childCount != 0 && _isDoneTalking)
            {
                Debug.Log("Npc end code running");
                GameObject currentObjectInHand = handPosition.GetChild(0).gameObject;
                List<GameObject> npcList = NpcManager.instance.currentNpcs;
                if (currentObjectInHand.TryGetComponent(out OrderInformation _orderInfo))
                {
                    int npcIndex = 1000;
                    for (int i = 0; i < npcList.Count; i++)
                    {
                        if (npcList[i] == hitNPC.transform.gameObject)
                        {
                            npcIndex = i;
                            Debug.Log($"Npc index is {i}");
                        }
                    }

                    if (npcIndex == 1000) return;
                    if (npcList[npcIndex].GetComponent<OrderInformation>().finalOrderType == _orderInfo.finalOrderType)
                    {
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("canEndOrder", true);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("isCorrect", true);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("OrderFinished", true);
                        Destroy(handPosition.GetChild(0));

                    }
                    else
                    {
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("canEndOrder", true);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("isCorrect", false);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("OrderFinished", true);
                        Destroy(handPosition.GetChild(0));
                    }


                }
            }
            

            #endregion

            npcBehaviour.BlackboardReference.GetVariable("canTalk", out BlackboardVariable<bool> _npcCanTalk);
            Debug.Log($"Npc can talk is equal to {_npcCanTalk}");
            if (!_npcCanTalk) return;
            npcBehaviour.BlackboardReference.GetVariable("isBeingTalkedTo", out BlackboardVariable<bool> _isBeingTalkedTo);
            _isBeingTalkedTo.Value = true;
            npcBehaviour.BlackboardReference.GetVariable("hasPressedTalk", out BlackboardVariable<bool> _hasPressedTalk);
            //SwitchBool
            if (_hasPressedTalk.Value) _hasPressedTalk.Value = true; else if (!_hasPressedTalk.Value) _hasPressedTalk.Value = false;
            StartCoroutine(DialogueCooldown(dialogueCooldownTimer));
           
        }

        //Pick Up Action
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

    #region Cooldown Timers

    IEnumerator DialogueCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canIncreaseDialogue = true;
    }

    #endregion

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
