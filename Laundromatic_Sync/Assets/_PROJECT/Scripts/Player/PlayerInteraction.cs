using System;
using System.Collections;
using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Behavior;
using Unity.VisualScripting;
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
    [SerializeField] LayerMask laptopLayer;
    [SerializeField] float dialogueCooldownTimer;

    [Title("User UI", null, TitleAlignments.Centered)]
    [SerializeField] TextMeshProUGUI objectDitectedString;
    [SerializeField] GameObject objectDitectedCanvas;
    [SerializeField] LayerMask nameInteractionLayers;
    [SerializeField] TextMeshProUGUI popupMessageText;
    [SerializeField] TextMeshProUGUI petableInfo;

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
        inputActions.Player.Pet.performed += x => PetAnimal();
        //TODO: Implement a way for the player to cancel the action
        //inputActions.Player.Back.performed += x => BackPressed();

        #endregion

        playerCam = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        ShowItemNameWhenRayHit();
    }

    void ShowItemNameWhenRayHit()
    {
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hit, raycastDistance, nameInteractionLayers))
        {
            if (hit.transform.TryGetComponent(out InteractionNameData _useableItem))
            {
                objectDitectedString.text = _useableItem.itemName;
                objectDitectedCanvas.SetActive(true);
            }
            if (hit.transform.TryGetComponent(out PetableThing petableThing))
            {
                petableInfo.text = "Press F to pet";
            }
        }
        else
        {
            petableInfo.text = "";
            objectDitectedString.text = "";
            objectDitectedCanvas.SetActive(false);
        }
    }

    void PetAnimal()
    {
        UpdatePlayerPopupMessage("Doggo says thank you for the pets!");
    }

    private void InteractWithObject()
    {
        //Interact With NPC
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitNPC, raycastDistance, npcLayer))
        {
            if (!hitNPC.transform.TryGetComponent(out BehaviorGraphAgent npcBehaviour)) return;
            npcBehaviour.BlackboardReference.SetVariableValue("isBeingTalkedTo", true);

            Debug.Log("NpcHit");
            npcBehaviour.BlackboardReference.GetVariableValue("IsDoneTalking", out bool _isDoneTalking);
            Debug.Log($"Is done talking got");
            if (npcBehaviour.BlackboardReference.GetVariableValue("IsDoneTalking", out bool _done))
            {
                Debug.Log("true");
            }
            else
            {
                Debug.Log("false");
            }

            #region NPC END CODE

            if (handPosition.childCount > 0 && _isDoneTalking)
            {
                Debug.Log("Npc end code running");
                GameObject currentObjectInHand = handPosition.GetChild(0).gameObject;
                List<GameObject> npcList = NpcManager.instance.currentNpcs;
                if (currentObjectInHand.TryGetComponent(out Basket _basketInfo))
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
                    if (_basketInfo.isCleanOrder)
                    {
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("canEndOrder", true);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("isCorrect", true);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("OrderFinished", true);
                        if (_basketInfo.ItemsUsed == ItemsUsed.FabricSoftner)
                        {
                            PlayerSettings.instance.AddMoney(50);
                        }
                        else
                        {
                            PlayerSettings.instance.AddMoney(20);
                        }
                        Destroy(handPosition.GetChild(0).gameObject);

                    }
                    else
                    {
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("canEndOrder", true);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("isCorrect", false);
                        npcBehaviour.BlackboardReference.SetVariableValue<bool>("OrderFinished", true);
                        PlayerSettings.instance.AddMoney(10);
                        Destroy(handPosition.GetChild(0).gameObject);
                    }


                }
            }


            #endregion

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
            //normal interaction
            if (hit.transform.TryGetComponent(out Iinteractable interactableObject))
            {
                interactableObject.Interact();
            }
        }

        //Place object on Table
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitTable, raycastDistance,
                tableLayer))
        {
            Debug.Log($"Table Hit");
            if (handPosition.childCount == 0) return;
            Transform hitObject = hitTable.transform;
            Transform handObject = handPosition.GetChild(0);
            for (int i = 0; hitObject.childCount > i; i++)
            {
                if (hitObject.GetChild(i).childCount == 0)
                {
                    handObject.SetParent(hitObject.GetChild(i));
                    handObject.transform.DOLocalMove(Vector3.zero, 0.5f);
                    handObject.GetComponent<Collider>().enabled = true;


                    break;
                }
            }
        }

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitLaptop, raycastDistance,
                laptopLayer)) {
            //InteractWithCamOnly
            if (hitLaptop.transform.TryGetComponent(out IMachineInteractable machineInteractable))
            {
                machineInteractable.Interact(playerCam);
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
        






    }

    #region Cooldown Timers

    IEnumerator DialogueCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canIncreaseDialogue = true;
    }

    #endregion

    public void UpdatePlayerPopupMessage(string messageInfo)
    {
        popupMessageText.text = messageInfo;
        StartCoroutine(TimeToWaitToDisablePopupText(3f));

    }

    public void StopAndStartPlayerMovement()
    {
        if (!playerMovement.canMoveAndLook)
        {
            playerMovement.canMoveAndLook = true;
            playerMovement.inputActions.Enable();
        }
        else
        {
            playerMovement.canMoveAndLook = false;
            playerMovement.inputActions.Disable();
        }
        
    }

        public void StopAndStartPlayerMovement(bool setToThis)
    {
        if (setToThis)
        {
            playerMovement.canMoveAndLook = true;
            playerMovement.inputActions.Enable();
        }
        else
        {
            playerMovement.canMoveAndLook = false;
            playerMovement.inputActions.Disable();
        }
        
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

    IEnumerator TimeToWaitToDisablePopupText(float time)
    {
        yield return new WaitForSeconds(time);
        popupMessageText.text = "";
    }
    
    
}

public enum InteractionType
{
    Pickup, Interact, Null
}
