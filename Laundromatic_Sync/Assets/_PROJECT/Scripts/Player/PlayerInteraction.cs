using System;
using System.Collections;
using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Behavior;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Title("Player", null, TitleAlignments.Centered)]
    [SerializeField] Transform handPosition;
    public Transform hatPosition;
    private PlayerMovement playerMovement;
    private InteractionType interactionType;
    [Title("Throwing Vars", null, TitleAlignments.Centered)]
    [Range(0f, 100f)]
    [SerializeField] float throwBaseForce;
    [SerializeField] float maxForceAmount;
    [SerializeField] float maxHoldTime;
    [Range(0f, 0.2f)]
    [SerializeField] float objectShakeIntensity;
    [SerializeField] float objectShakeRandomness;
    float heldAmount = 0;
    float shootAmount = 0;
    [Title("Camera Shake Vars", null, TitleAlignments.Centered)]
    [Range(0f, 0.2f)]
    [SerializeField] float camShakeStrenght;
    [SerializeField] float camShakeRandomness;

    #region Interaction Variables

    [Title("Interaction Vars", null, TitleAlignments.Centered)]
    [Range(0f, 10f)]
    public float raycastDistance;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] LayerMask tableLayer;
    [SerializeField] LayerMask npcLayer;
    [SerializeField] LayerMask laptopLayer;
    [SerializeField] float dialogueCooldownTimer;

    [Title("User UI", null, TitleAlignments.Centered)]
    [SerializeField] TextMeshProUGUI objectDitectedString;
    [SerializeField] GameObject objectDitectedCanvas;
    [SerializeField] GameObject playerCroshair;
    [SerializeField] LayerMask nameInteractionLayers;
    [SerializeField] TextMeshProUGUI popupMessageText;
    [SerializeField] TextMeshProUGUI petableInfo;

    Camera playerCam;

    #endregion

    InputSystem_Actions inputActions;
    bool canIncreaseDialogue = true;
    public bool canInteract = true;
    Rigidbody playerRb;


    private void Start()
    {
        #region Input Actions

        inputActions = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
        inputActions.Enable();
        inputActions.Player.Interact.performed += x => InteractWithObject();
        //inputActions.Player.Pet.performed += x => PetAnimal();
        //TODO: Implement a way for the player to cancel the action
        //inputActions.Player.Back.performed += x => BackPressed();
        inputActions.Player.DropItem.performed += DropItem;
        inputActions.Player.DropItem.canceled += x => ThrowItem();
        inputActions.Player.DropHat.performed += DropHat;
        inputActions.Player.DropHat.canceled += x => ThrowHat();

        #endregion

        playerCam = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
    }

    #region Interaction Visuals

    void Update()
    {
        if (!canInteract)
        {
            objectDitectedString.text = "";
            playerCroshair.SetActive(false);
            return;
        }
        else
        {
            ShowItemNameWhenRayHit();
            playerCroshair.SetActive(true);
        }

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

    #endregion

    #region Pet

    void PetAnimal()
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, raycastDistance);
        string nameOfObject = hit.transform.name;
        if (hit.transform.TryGetComponent(out InteractionNameData nameData))
        {
            nameOfObject = nameData.name;
        }

        if (nameOfObject == null) return;
        UpdatePlayerPopupMessage($"{nameOfObject} says thank you for the pets!");
    }

    #endregion

    #region Interactions
    private void InteractWithObject()
    {
        if (!canInteract) return;
        //Interact With NPC
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitNPC, raycastDistance, npcLayer))
        {
            if (!hitNPC.transform.TryGetComponent(out BehaviorGraphAgent npcBehaviour)) return;
            npcBehaviour.BlackboardReference.SetVariableValue("isBeingTalkedTo", true);
            npcBehaviour.BlackboardReference.SetVariableValue<Transform>("Player", this.transform);

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
                    if (hitNPC.transform.TryGetComponent(out NpcOrderData npcOrderIndfo))
                    {
                        if (_basketInfo.npcID == npcOrderIndfo.npcID && _basketInfo.BasketType == BasketType.Folded)
                        {
                            npcBehaviour.BlackboardReference.SetVariableValue<bool>("canEndOrder", true);
                            npcBehaviour.BlackboardReference.SetVariableValue<bool>("isCorrect", true);
                            npcBehaviour.BlackboardReference.SetVariableValue<bool>("OrderFinished", true);
                            ++DayNight.instance.ordersCompletedToday;
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
                            ++DayNight.instance.ordersWrongToday;
                            PlayerSettings.instance.AddMoney(10);
                            //Destroy(handPosition.GetChild(0).gameObject);



                        }
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
            int forAmount = hitObject.childCount;
            --forAmount;
            for (int i = 0; forAmount > i; i++)
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
                laptopLayer))
        {
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

    void BackPressed()
    {
        if (!playerMovement.canMoveAndLook)
        {
            playerMovement.ResetCamera();
            playerMovement.canMoveAndLook = true;
        }

    }

    

    #endregion

    #region Throw and Drop

    void DropItem(InputAction.CallbackContext obj)
    {
        if (handPosition.childCount == 0) return;
        heldAmount = Time.time;
        GameObject objectInHand = handPosition.GetChild(0).gameObject;
        Rigidbody objectRigidbody = objectInHand.GetComponent<Rigidbody>();
        objectRigidbody.constraints = RigidbodyConstraints.None;
        objectRigidbody.isKinematic = false;
        Debug.Log($"[PlayerInteraction] {heldAmount}");
        objectInHand.transform.DOShakePosition(100f, objectShakeIntensity, randomness: objectShakeRandomness);
        Camera.main.transform.DOShakePosition(100f, camShakeStrenght, randomness: camShakeRandomness, randomnessMode: ShakeRandomnessMode.Harmonic);
    }

    void ThrowItem()
    {
        if (handPosition.childCount == 0) return;
        GameObject objectInHand = handPosition.GetChild(0).gameObject;
        objectInHand.transform.DOKill();
        Camera.main.transform.DOKill();
        Rigidbody objectRigidbody = objectInHand.GetComponent<Rigidbody>();

        Collider collier = objectInHand.GetComponent<Collider>();
        float finalTime = Time.time - heldAmount;
        float percentageHeld = 0f;
        if (finalTime >= maxHoldTime)
        {
            percentageHeld = 1f;
        }
        else
        {
            percentageHeld = finalTime / maxHoldTime;
            Debug.Log($"[PlayerInteraction] {percentageHeld}");
        }

        float finalforce = maxForceAmount * percentageHeld;


        float finalForceAmount = Mathf.Clamp(finalforce, throwBaseForce, maxForceAmount);
        Debug.Log($"[PlayerInteraction] final force is: {finalForceAmount}");

        collier.enabled = true;
        objectInHand.transform.parent = null;
        objectRigidbody.isKinematic = false;
        objectRigidbody.useGravity = true;

        objectRigidbody.linearVelocity = playerRb.linearVelocity;
        objectRigidbody.linearVelocity = Camera.main.transform.forward * finalForceAmount;
        //objectRigidbody.AddForce(Camera.main.transform.forward * finalForceAmount, ForceMode.Force);
        Debug.Log($"[PlayerInteraction] {finalTime}");
    }

    void DropHat(InputAction.CallbackContext obj)
    {
        if (hatPosition.childCount == 0) return;
        heldAmount = Time.time;
        GameObject hatObject = hatPosition.GetChild(0).gameObject;
        Rigidbody hatRigidbody = hatObject.GetComponent<Rigidbody>();
        hatRigidbody.constraints = RigidbodyConstraints.None;
        hatRigidbody.isKinematic = false;
        Debug.Log($"[PlayerInteraction] {heldAmount}");
        hatObject.transform.DOShakePosition(100f, objectShakeIntensity, randomness: objectShakeRandomness);
        Camera.main.transform.DOShakePosition(100f, camShakeStrenght, randomness: camShakeRandomness, randomnessMode: ShakeRandomnessMode.Harmonic);
    }

    void ThrowHat()
    {
        if (hatPosition.childCount == 0) return;
        GameObject hatObject = hatPosition.GetChild(0).gameObject;
        hatObject.transform.DOKill();
        Camera.main.transform.DOKill();
        Rigidbody objectRigidbody = hatObject.GetComponent<Rigidbody>();

        Collider collier = hatObject.GetComponent<Collider>();
        float finalTime = Time.time - heldAmount;
        float percentageHeld = 0f;
        if (finalTime >= maxHoldTime)
        {
            percentageHeld = 1f;
        }
        else
        {
            percentageHeld = finalTime / maxHoldTime;
            Debug.Log($"[PlayerInteraction] {percentageHeld}");
        }

        float finalforce = maxForceAmount * percentageHeld;


        float finalForceAmount = Mathf.Clamp(finalforce, throwBaseForce, maxForceAmount);
        Debug.Log($"[PlayerInteraction] final force is: {finalForceAmount}");

        collier.enabled = true;
        hatObject.transform.parent = null;
        objectRigidbody.isKinematic = false;
        objectRigidbody.useGravity = true;

        objectRigidbody.linearVelocity = playerRb.linearVelocity;
        objectRigidbody.linearVelocity = Camera.main.transform.forward * finalForceAmount;
        //objectRigidbody.AddForce(Camera.main.transform.forward * finalForceAmount, ForceMode.Force);
        Debug.Log($"[PlayerInteraction] {finalTime}");
    }

    #endregion

    #region Cooldown Timers

    IEnumerator DialogueCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canIncreaseDialogue = true;
    }

    #endregion

    #region Outside Funcs
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
            playerCroshair.SetActive(true);
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

    public void StopAndStartPlayerInteraction(bool setToThis)
    {
        if (setToThis)
        {
            inputActions.Enable();
        }
        else
        {
            inputActions.Disable();
        }
    }

    public MachineType CheckMachineType(IMachineInteractable interactableMachine)
    {
        Debug.Log(interactableMachine.MachineType());
        return interactableMachine.MachineType();

    }
    #endregion



    BasketType CheckBasketType(RaycastHit hit)
    {
        return hit.transform.TryGetComponent(out Basket basket)
            ? basket.ReturnBasketType()
            : BasketType.None;
    }

    #region End

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    IEnumerator TimeToWaitToDisablePopupText(float time)
    {
        yield return new WaitForSeconds(time);
        popupMessageText.text = "";
    }
    
    #endregion
    
    
}

public enum InteractionType
{
    Pickup, Interact, Null
}
