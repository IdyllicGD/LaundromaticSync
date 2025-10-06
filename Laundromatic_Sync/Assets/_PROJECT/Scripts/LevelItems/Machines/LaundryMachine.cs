using System;
using System.Collections;
using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using _PROJECT.Scripts.Rhythm;
using UnityEngine.UIElements;
using FMODUnity;
using Sirenix.OdinInspector;
using TMPro;
using System.Threading;

public class LaundryMachine : MonoBehaviour, IMachineInteractable, Iinteractable, RythmStages
{
    Animator animator;
    GameObject currentObject;
    PlayerMovement currentPlayerMovement;
    [Title("Wash time", null, TitleAlignments.Centered)]
    [SerializeField] float machineTimeToComplete;
    [SerializeField] float maxMachineTimeToComplete;
    float originalMachineTimeToComplete;

    [Title("Transforms", null, TitleAlignments.Centered)]
    [SerializeField] Transform camPosition;
    [SerializeField] Transform camLookAtPosition;
    [SerializeField] Transform basketPosition;

    [Title("Button Info", null, TitleAlignments.Centered)]
    [SerializeField] ButtonSpawner buttonSpawner;

    [Title("Prefabs", null, TitleAlignments.Centered)]
    [SerializeField] GameObject cleanBasket;

    [Title("Sounds", null, TitleAlignments.Centered)]
    [SerializeField] StudioEventEmitter doneSound;
    [SerializeField] StudioEventEmitter busySound;
    [SerializeField] StudioEventEmitter buttonSound;
    [SerializeField] StudioEventEmitter doorOpenSound;
    [SerializeField] StudioEventEmitter doorCloseSound;

    [Title("Enums", null, TitleAlignments.Centered)]
    [SerializeField] CurrentStage currentStage;
    [Title("RaycastPositions", null, TitleAlignments.Centered)]
    [SerializeField] Transform raycastPosition;
    [Title("Animatiom", null, TitleAlignments.Centered)]
    [SerializeField] ParticleSystem bubbles;
    [Title("Canvas", null, TitleAlignments.Centered)]
    [SerializeField] TextMeshProUGUI TimeText;
    [SerializeField] GameObject mainCan;
    [SerializeField] UiTimerMachines uiTimer;





    public MachineType machineType;


    #region Needed Public Vars

    [Title("Bools", "Only for debugging", TitleAlignments.Centered)]
    [ReadOnly] public bool hasDetergent;
    [ReadOnly] public bool hasFabricSoftener;
    [ReadOnly] public bool isBusy;
    [ReadOnly] public bool isDone;

    #endregion

    #region Machine Head Vars

    private float baseSpeedCam;
    private float baseSpeedSecondObject;
    private float baseLookAtPositionSpeed;

    #endregion

    void Update()
    {

        if (!isDone) return;
        if (HasObject()) return;
        animator.SetBool("isFinished", false);
        animator.SetBool("isIdle", true);
        uiTimer.ResetUiTimer();
        isDone = false;
    }

    public bool IsBusy()
    {
        return isBusy;
    }
    private void Awake()
    {
        currentStage = CurrentStage.FillingUpStage;
        originalMachineTimeToComplete = machineTimeToComplete;
        TimeText.text = machineTimeToComplete.ToString();

        #region Machine Head

        //TODO: Refactor this later to not use magic numbers
        baseSpeedCam = 0.5f;
        baseSpeedSecondObject = 0.5f;
        baseLookAtPositionSpeed = 0.5f;

        #endregion
        animator = transform.GetComponentInParent<Animator>();

        animator.SetBool("isIdle", true);

    }

    public void Interact()
    {
        Debug.Log($"[Laundry Machine] Interact With Object");

    }

    //Interact With machine and just moving the cam
    public void Interact(Camera camObject)
    {
        Debug.Log($"[LaundryMachine] just move cam Interaction Func");
    }

    void StartStage(CurrentStage stage)
    {
        switch (stage)
        {
            case CurrentStage.FillingUpStage:
                break;
            case CurrentStage.WashingStage:
                mainCan.SetActive(true);
                uiTimer.hasBasket = true;
                uiTimer.UpdateImages();
                StartButtonSpawning();

                //Start beeps and boops
                break;
            case CurrentStage.FinishedStage:
                //exit to normal view
                busySound.Play();
                buttonSound.Play();
                bubbles.Play();
                animator.SetTrigger("StartCycle");
                animator.SetBool("isIdle", false);
                uiTimer.StartTimerTime(machineTimeToComplete);
                StartCoroutine(EndMachineFunc(machineTimeToComplete, currentObject));
                
                mainCan.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
        }
    }

    void StartButtonSpawning()
    {

        buttonSpawner.currentStage = 0;
        buttonSpawner.SpawnButton();



    }

    bool HasObject()
    {
        Debug.DrawRay(raycastPosition.position, Vector3.down, Color.blue, 1f);
        if (basketPosition.childCount >= 1)
        {
            return true;
        }

        return false;

    }


    #region Interact with machine and move cam and another object. Example: Basket
    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        if (HasObject()) return;

        currentObject = secondaryObject;
        PlayerInteraction playerInteraction = camObject.transform.parent.GetComponentInParent<PlayerInteraction>();
        currentPlayerMovement = camObject.transform.parent.GetComponentInParent<PlayerMovement>();

        Debug.Log($"[Laundry Machine] {ReturnItemType(secondaryObject)}");
        Debug.Log("[LaundryMachine] Trying to setTrigger");
        animator.SetTrigger("DtFs");
        doorOpenSound.Play();
        //doorCloseSound.Play();
        switch (ReturnItemType(secondaryObject))
        {
            //Washing Any Type
            case MachineUsableItemsType.None:
                Debug.Log($"[Laundry Machine] You need Detergent first");
                playerInteraction.UpdatePlayerPopupMessage("You need Detergent first before you can wash this");
                if (currentStage == CurrentStage.FillingUpStage && hasDetergent)
                {
                    currentStage++;
                    currentPlayerMovement.cameraOriginalTransform = Camera.main.transform.position;
                    currentPlayerMovement.cameraOriginalRotation = Camera.main.transform.rotation;
                    playerInteraction.StopAndStartPlayerMovement();
                    playerInteraction.canInteract = false;
                    camObject.transform.DOMove(camPosition.position, baseSpeedCam);
                    camObject.transform.DODynamicLookAt(camLookAtPosition.position, baseLookAtPositionSpeed);
                    secondaryObject.transform.SetParent(basketPosition);
                    secondaryObject.transform.DOLocalMove(Vector3.zero, baseSpeedSecondObject).OnComplete(() => StartStage(currentStage));
                }
                return;
            //Washing Baskets
            case MachineUsableItemsType.Basket:
                //Checks if the basket type is dirty, is the machine has Detergent and if the machine is in the correct state
                switch (hasDetergent)
                {
                    case true:
                        if (ReturnBasketType(secondaryObject) != BasketType.Dirty)
                        {
                            Debug.Log($"[Laundry Machine] You a dirty basket!");
                        }
                        break;
                    case false:
                        Debug.Log($"[Laundry Machine] You need Detergent first!");
                        playerInteraction.UpdatePlayerPopupMessage("You need detergent first");
                        break;
                }

                if (ReturnBasketType(secondaryObject) == BasketType.Dirty && hasDetergent)
                {
                    if (currentStage == CurrentStage.FillingUpStage)
                    {
                        currentStage = CurrentStage.WashingStage;
                        currentPlayerMovement.cameraOriginalRotation = Camera.main.transform.rotation;
                        currentPlayerMovement.cameraOriginalTransform = Camera.main.transform.position;
                        playerInteraction.StopAndStartPlayerMovement();
                        playerInteraction.canInteract = false;
                        camObject.transform.DOMove(camPosition.position, baseSpeedCam);
                        camObject.transform.DODynamicLookAt(camLookAtPosition.position, baseLookAtPositionSpeed);
                        secondaryObject.transform.SetParent(basketPosition);
                        secondaryObject.transform.DOLocalMove(Vector3.zero, baseSpeedSecondObject).OnComplete(() => StartStage(currentStage));
                    }
                }


                break;
            case MachineUsableItemsType.Detergent:
                if (hasDetergent)
                {
                    Debug.Log($"[Laundry Machine] Machine already has detergent");
                    playerInteraction.UpdatePlayerPopupMessage("This machine already has detergent");
                    break;
                }
                if (currentStage == CurrentStage.FillingUpStage)
                {
                    Destroy(secondaryObject);
                    hasDetergent = true;
                    uiTimer.hasDetergent = true;
                    UpdateInfoCards();
                }
                break;
            case MachineUsableItemsType.FabricSoftener:
                if (hasFabricSoftener)
                {
                    Debug.Log($"[Laundry Machine] Machine already has fabric softner");
                    playerInteraction.UpdatePlayerPopupMessage("This machine already has fabric softener");
                    break;
                }
                if (currentStage == CurrentStage.FillingUpStage)
                {
                    Destroy(secondaryObject);
                    hasFabricSoftener = true;
                    uiTimer.hasFabricSoftener = true;
                    UpdateInfoCards();
                }
                break;
            default:
                Debug.Log($"[Laundry Machine] This item cannot be used on this machine");
                playerInteraction.UpdatePlayerPopupMessage("This item cannot be used with this machine");
                break;
        }

    }
    #endregion

    #region Return Functions

    MachineUsableItemsType ReturnItemType(GameObject itemObject)
    {
        return !itemObject.TryGetComponent(out IMachineUsableItems interactable)
            ? MachineUsableItemsType.None
            : interactable.ReturnMachineUsableItemsType();
    }

    BasketType ReturnBasketType(GameObject basket)
    {
        return !basket.TryGetComponent(out Basket basketScript)
            ? BasketType.None
            : basketScript.ReturnBasketType();
    }

    public MachineType MachineType()
    {
        return machineType;
    }


    #endregion




    IEnumerator EndMachineFunc(float waitTime, GameObject itemType)
    {
        yield return new WaitForSeconds(waitTime);
        bubbles.Stop();
        currentStage = CurrentStage.FillingUpStage;
        switch (ReturnItemType(itemType))
        {
            case MachineUsableItemsType.None:
                Destroy(transform.parent.GetChild(0).GetChild(0).gameObject);
                GameObject _spawningItem = Instantiate(itemType, basketPosition);
                _spawningItem.SetActive(true);
                _spawningItem.GetComponent<PickupItem>().cleanOrDirty = CleanOrDirty.Clean;
                _spawningItem.GetComponent<Collider>().enabled = true;
                break;
            case MachineUsableItemsType.Basket:
                Basket currentBasket;
                currentBasket = transform.parent.GetChild(0).GetChild(0).gameObject.GetComponent<Basket>();
                currentBasket.UpdateBasketInfoWet();
                currentBasket.gameObject.SetActive(true);
                if (hasFabricSoftener)
                {
                    currentBasket.GetComponent<Basket>().ItemsUsed = ItemsUsed.FabricSoftner;
                }
                break;
        }

        hasDetergent = false;
        hasFabricSoftener = false;
        animator.SetBool("isFinished", true);
        isDone = true;
        busySound.Stop();
        doneSound.Play();
        machineTimeToComplete = originalMachineTimeToComplete;
        TimeText.text = machineTimeToComplete.ToString();


    }

    public void StageCheck()
    {
        if (buttonSpawner.currentStage == buttonSpawner.spawnPoints.Count)
        {
            currentStage = 0;
            currentStage = CurrentStage.FinishedStage;
            StartStage(currentStage);
            if (currentPlayerMovement != null)
            {
                Debug.Log("CurrentPlayerMovement is null");
            }
            else
            {
                Debug.Log("Current player movement is not null");
            }

            currentPlayerMovement.ResetCamera();
            transform.parent.GetChild(0).GetChild(0).gameObject.SetActive(false);
            //Exit focus mode
            return;
        }
        buttonSpawner.SpawnButton();
        //Play current animation where needed

    }

    public void AddTimeToTimer(float amountToAdd)
    {
        Debug.Log($"[LaundryMachine] Add to timer timer is now: {machineTimeToComplete}");
        if (machineTimeToComplete >= maxMachineTimeToComplete) return;
        machineTimeToComplete += amountToAdd;
        TimeText.text = machineTimeToComplete.ToString();
    }

    void UpdateInfoCards()
    {
        uiTimer.UpdateImages();
    }

}

public enum CurrentStage{
        FillingUpStage, WashingStage, FinishedStage, DryingStage
    }
    
    