using System.Collections;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.Rhythm;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using FMODUnity;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TimerMachines : MonoBehaviour, IMachineInteractable, RythmStages
{
    //This script is for the laundry machine and dryer machine.
    //It should be flexable with the time it takes and with alsmot all functions seperate to share them amongst other machines
    //Should be easily upgradeable with just changing variables
    [Title("Testing Vars")]
    public bool skipRythm;



    [Title("Outside Vars")]
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerMovement playerMovement;




    [Title("Macine info", null, TitleAlignments.Centered)]
    [SerializeField] TypeOfMachine machineType;
    [SerializeField] BasketType correctBasket;
    [SerializeField] BasketType nextBasketType;
    [SerializeField] float timerTime;
    [SerializeField] float objectMoveTime;
    [SerializeField] Transform objectCheckRaycastPosition;




    [Title("DEBUG", null, TitleAlignments.Centered)]
    [ReadOnly] bool hasDetergent;
    [ReadOnly] bool hasFabricSoftener;
    [ReadOnly] bool hasBasket;




    [Title("Rythm Items", null, TitleAlignments.Centered)]
    [SerializeField] ButtonSpawner buttonSpawner;
    [SerializeField] int rounds;
    [SerializeField] int timeToAddToTimer;
    [SerializeField] GameObject RythmInfoCanvas;
    [SerializeField] TextMeshProUGUI TimeTextRythm;

    [Title("Start Machine Items", null, TitleAlignments.Centered)]
    [SerializeField] Transform basketPlacePosition;
    [SerializeField] Transform camLookPosition;
    [SerializeField] Transform camPosition;
    [SerializeField] TextMeshProUGUI timerTimeText;
    [SerializeField] GameObject timerInfo;
    [SerializeField] GameObject timerHud;
    [SerializeField] float machineTimeToComplete;
    [SerializeField] float maxMachineTimeToComplete;




    [Title("Sounds", null, TitleAlignments.Centered)]
    [SerializeField] StudioEventEmitter doneSound;
    [SerializeField] StudioEventEmitter busySound;
    [SerializeField] StudioEventEmitter buttonSound;
    [SerializeField] StudioEventEmitter doorOpenSound;
    [SerializeField] FMODEventTrack playThis;

    [Title("Animation", null, TitleAlignments.Centered)]
    [SerializeField] Animator animator;
    [Title("Timer", null, TitleAlignments.Left, false)]
    [SerializeField] TextMeshProUGUI addTimerAnimText;
    [SerializeField] float animDuration;



    [Title("Colours on Machine", null, TitleAlignments.Centered)]
    [SerializeField] Color hasObjectColour;
    [SerializeField] Color doesntHaveObjectColour;
    [SerializeField] Image detergentColour, fabricSoftenerColour, basketColour;

    [Title("Canvasas")]
    public GameObject canvasTimer, canvasGroup;


    [SerializeField] GameObject currentBasket;
    [SerializeField] ParticleSystem currentPs;
    bool isDone;
    bool wasThrown;
    public bool interupt;
    bool addTimeBusy;
    float originalMachineTimer;
    GameObject currentFabricSoftener;
    GameObject currentDetergent;

    void Start()
    {
        originalMachineTimer = timerTime;
        machineTimeToComplete = timerTime;
    }

    void OnTriggerEnter(Collider targetObject)
    {
        if (isBusy) return;
        if (machineType == TypeOfMachine.DryingMachine && hasBasket) return;
        if (targetObject.TryGetComponent(out Basket basket))
        {
            if (basket.BasketType == correctBasket)
            {
                currentBasket = targetObject.gameObject;
                Interact(Camera.main, targetObject.gameObject, true);
            }
            else
            {
                return;
            }
        }
        if(targetObject.TryGetComponent(out MachineUsableObjects currentMachineUseableObject) && machineType == TypeOfMachine.LaundryMachine)
        {
            if(currentMachineUseableObject.ReturnMachineUsableItemsType() == MachineUsableItemsType.Detergent)
            {
                currentDetergent = targetObject.gameObject;
                Interact(Camera.main, targetObject.gameObject, true);
                
            }else if(currentMachineUseableObject.ReturnMachineUsableItemsType() == MachineUsableItemsType.FabricSoftener)
            {
                currentFabricSoftener = targetObject.gameObject;
                Interact(Camera.main, targetObject.gameObject, true);
                
            }
            else
            {
                return;
            }
        } 

    }



    void Update()
    {
        if (!isDone) return;
        if (HasObject()) return;
        AnimSetToIdel();
        timerTimeText.text = "";
        hasBasket = false;
        UpdateImages();
        isDone = false;
        timerTimeText.text = "";
        interupt = false;
        currentBasket = null;
    }

    bool HasObject()
    {
        //Debug.DrawRay(objectCheckRaycastPosition.position, Vector3.down, Color.blue, 1f);
        if (basketPlacePosition.childCount >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
        

    }

    bool isBusy = false;
    public void Interact(Camera camObject)
    {
        if (isBusy) return;
        if (currentBasket == null) return;
        if (currentBasket.GetComponent<Basket>().BasketType != correctBasket) return;
        switch (machineType)
        {
            case TypeOfMachine.LaundryMachine:
                if (hasBasket && hasDetergent)
                {
                    if (skipRythm)
                    {
                        StartMachine();
                    }
                    else
                    {
                        StartRythm();
                    }
                }
                else
                {
                    PopupMessageMachineItems();
                }
                break;
            case TypeOfMachine.DryingMachine:
                if (hasBasket)
                {
                    if (skipRythm)
                    {
                        StartMachine();
                    }
                    else
                    {
                        StartRythm();
                    }
                }
                break;
        }
    
            

    }

    public void Interact(Camera camObject, GameObject secondaryObject, bool wasItemThrown)
    {
        doneSound.Stop();
        if (isBusy) return;
        wasThrown = wasItemThrown;
        if(machineType == TypeOfMachine.LaundryMachine && hasDetergent && hasBasket)
        {
            if(secondaryObject.TryGetComponent(out MachineUsableObjects itemTypeCurrent))
            {
                if(itemTypeCurrent.ReturnMachineUsableItemsType() == MachineUsableItemsType.FabricSoftener)
                {
                    if (hasFabricSoftener && !wasItemThrown)
                    {
                        Interact(camObject);
                        return;
                    }
                    AnimOpenDoor();
                    hasFabricSoftener = true;
                    UpdateImages();
                    if (currentFabricSoftener == null)
                    {
                        DestroyObjectInPlayerhand();
                        return;
                    }
                    else
                    {
                        Destroy(currentFabricSoftener);    
                        currentFabricSoftener = null;
                        return;
                    }
                }else if(machineType == TypeOfMachine.LaundryMachine && !wasItemThrown)
                {
                    Interact(camObject);
                    return;
                }
                
                
            }
        }
        // else
        // {
        //     Interact(camObject);
        //     return;
        // }
        if (machineType == TypeOfMachine.DryingMachine && hasBasket && !wasItemThrown)
        {
            Interact(camObject);
            return;
        }

        if (secondaryObject.TryGetComponent(out MachineUsableObjects itemType))
        {
            MachineUsableItemsType _type = itemType.ReturnMachineUsableItemsType();

                switch (_type)
                {
                case MachineUsableItemsType.Detergent:
                    if (machineType != TypeOfMachine.LaundryMachine) return;
                        if (hasDetergent)
                        {
                            playerInteraction.UpdatePlayerPopupMessage($"Machine already has {_type}");
                            break;
                        }
                        else
                        {
                            //TODO: make the thingy light up
                            AnimOpenDoor();
                            hasDetergent = true;
                            UpdateImages();
                            UpdateImages();
                            if (currentDetergent == null)
                            {
                                DestroyObjectInPlayerhand();
                            }
                            else
                            {
                                Destroy(currentDetergent);
                                currentDetergent = null;
                            }
                            //UpdateInfo
                            break;
                        }
                case MachineUsableItemsType.FabricSoftener:
                    if (machineType != TypeOfMachine.LaundryMachine) return;
                        if (hasFabricSoftener)
                        {
                            playerInteraction.UpdatePlayerPopupMessage($"Machine already has {_type}");
                            break;
                        }
                        else
                        {
                            AnimOpenDoor();
                            hasFabricSoftener = true;
                            UpdateImages();
                            if (currentFabricSoftener == null)
                            {
                                DestroyObjectInPlayerhand();
                            }
                            else
                            {
                                Destroy(currentFabricSoftener);
                                
                                currentFabricSoftener = null;
                            }

                            //UpdateInfo
                            break;
                        }
                
                case MachineUsableItemsType.Basket:
                    //Check if basket type is the correct one
                    Basket basket = secondaryObject.GetComponent<Basket>();
                    BasketType type = basket.BasketType;
                    if (hasBasket)
                    {
                        playerInteraction.UpdatePlayerPopupMessage($"Machine already has {_type}");
                        break;
                    }
                    else
                    {
                        CheckIfCorrectBasket(type, secondaryObject);
                        break;
                    }
                case MachineUsableItemsType.None:
                    playerInteraction.UpdatePlayerPopupMessage($"Cant Use this item with this machine");
                    break;
            }
        }
        


    }

    void DestroyObjectInPlayerhand()
    {
        Destroy(playerInteraction.handPosition.GetChild(playerInteraction.currentActiveObjectIndex - 1).gameObject);
        playerInteraction.MoveItemsOneDown();
    }

    void CheckIfCorrectBasket(BasketType basketToCheck, GameObject basketGameObject)
    {
        if (basketToCheck == correctBasket)
        {
            AnimOpenDoor();
            hasBasket = true;
            MoveItemInPlayerHandToPosition(basketPlacePosition);
            UpdateImages();
        }
        else
        {
            playerInteraction.UpdatePlayerPopupMessage($"Wrong basket type! you need {correctBasket}");
        }
    }
    
    void MoveItemInPlayerHandToPosition(Transform endPostion)
    {
        if(currentBasket == null)
        {
            currentBasket = playerInteraction.handPosition.GetChild(playerInteraction.currentActiveObjectIndex - 1).gameObject;
        }
        GameObject playerHandObject = currentBasket;
        Rigidbody basketRb = currentBasket.GetComponent<Rigidbody>();
        currentBasket.GetComponent<Collider>().enabled = false;
        basketRb.isKinematic = true;
        basketRb.useGravity = false;
        playerHandObject.transform.parent = endPostion;
        playerHandObject.transform.DOLocalMove(Vector3.zero, objectMoveTime);
        playerHandObject.transform.DOLocalRotate(Vector3.zero, objectMoveTime);
        if(wasThrown)return;
        playerInteraction.MoveItemsOneDown();
    }

    #region Rythm
    void StartRythm()
    {
         
        playerMovement.cameraOriginalRotation = Camera.main.transform.rotation;
        playerMovement.cameraOriginalTransform = Camera.main.transform.position;
        playerInteraction.canInteract = false;
        RythmInfoCanvas.SetActive(true);
        playerInteraction.StopAndStartPlayerMovement(false);
        Camera.main.transform.DOMove(camPosition.position, objectMoveTime);
        buttonSpawner.currentStage = 0;
        Camera.main.transform.DODynamicLookAt(camLookPosition.position, objectMoveTime).OnComplete(() => buttonSpawner.SpawnButton());
        TimeTextRythm.text = timerTime.ToString("00");

        //Start Rythm
        //Take player out
        //Start Machine

    }

    public void StageCheck()
    {
        //rounds
        if (buttonSpawner.currentStage == buttonSpawner.spawnPoints.Count)
        {
            StartMachine();
            playerMovement.ResetCamera();
            transform.parent.GetChild(0).GetChild(0).gameObject.SetActive(false);
            RythmInfoCanvas.SetActive(false);
            return;
            //Reset cam
        }
        buttonSpawner.SpawnButton();



    }

    public void AddTimeToTimer(float timeToAdd)
    {

        #region Animation
        if (addTimeBusy)
        {
            addTimerAnimText.DOKill();
            addTimerAnimText.transform.DOKill();
            ResetTimerUi();
            addTimeBusy = false;
        }

        addTimeBusy = true;
        addTimerAnimText.text = $"+{timeToAdd}";
        addTimerAnimText.transform.DOLocalMoveY(90f, animDuration);
        addTimerAnimText.DOFade(0f, animDuration).OnComplete(() => ResetTimerUi());

        #endregion

        Debug.Log("[TimerMachines] add time to timer");
        if (timerTime >= maxMachineTimeToComplete) return;
        timerTime += timeToAdd;
        TimeTextRythm.text = timerTime.ToString();
    }
    
    void ResetTimerUi()
    {
        addTimerAnimText.transform.DOLocalMoveY(23f, 0f);
        addTimerAnimText.text = string.Empty;
        addTimerAnimText.DOFade(1f, 0f);
        addTimeBusy = false;
    }

    #endregion

    #region Start Machine

    void StartMachine()
    {
        currentPs.Play();
        TimeTextRythm.text = timerTime.ToString("00");
        currentBasket = basketPlacePosition.GetChild(0).gameObject;
        currentBasket.SetActive(false);
        Basket _currentBasket = currentBasket.GetComponent<Basket>();
        if (_currentBasket.BasketType != correctBasket) return;
        //play animations
        AnimMachineWashing();
        switch (_currentBasket.BasketType)
        {
            case BasketType.Dirty:
                _currentBasket.UpdateBasketInfoWet();
                if (hasFabricSoftener)
                {
                    _currentBasket.ItemsUsed = ItemsUsed.FabricSoftner;
                }
                break;
            case BasketType.WetClean:
                _currentBasket.UpdateBasketInfoDry();
                break;
            case BasketType.DryClean:
                _currentBasket.UpdateBasketInfoFolded();
                break;
            case BasketType.Folded:
                break;
            case BasketType.None:
                break;
        }
        //Change basket info
        isBusy = true;
        StartCoroutine(StartMachineTimer());
        //doneSound.Play();

    }

    IEnumerator StartMachineTimer()
    {
        
        isBusy = true;
        currentBasket.SetActive(false);
        while (timerTime > 0 && !interupt)
        {
            timerTime -= Time.deltaTime;
            timerTimeText.text = timerTime.ToString("00");
            Debug.Log($"Time: {timerTime}");
            yield return null;
        }
        MachineStop();

    }
    
    #endregion
    
    #region End

    void MachineStop()
    {
        currentPs.Stop();
        busySound.Stop();
        //doneSound.Stop();
        currentBasket.GetComponent<Collider>().enabled = true;
        Rigidbody basketRb = currentBasket.GetComponent<Rigidbody>();
        basketRb.isKinematic = false;
        basketRb.useGravity = true;
        
        timerTime = originalMachineTimer;
        AnimMachineFinished();
        currentBasket.SetActive(true);
        currentBasket = null;
        ResetMachineCompletely();
        isBusy = false;
    }
    #endregion

    public void ResetMachine()
    {
        isDone = true;
        timerTimeText.text = "";
        UpdateImages();
    }

    public void ResetMachineCompletely()
    {
        isDone = true;
        hasDetergent = false;
        hasFabricSoftener = false;
        timerTimeText.text = "";
        UpdateImages();
    }


    void PopupMessageMachineItems()
    {
        string outputString = $"You need:\n";
        if (!hasBasket)
        {
            outputString += "Dirty basket \n";
        }
        if (!hasDetergent)
        {
            outputString += "Detergent \n";
        }

        playerInteraction.UpdatePlayerPopupMessage(outputString);
    }

    public bool IsBusy()
    {
        return isBusy;
    }

    public MachineType MachineType()
    {
        throw new System.NotImplementedException();
    }

    #region animations and sounds combined

    void AnimOpenDoor()
    {
        animator.SetBool("isIdle", false);
        animator.SetTrigger("DtFs");
        doorOpenSound.Play();
    }

    void AnimMachineWashing()
    {
        animator.SetTrigger("StartCycle");
        busySound.Play();
    }

    void AnimMachineFinished()
    {
        busySound.Stop();
        animator.SetBool("isFinished", true);
        doneSound.Play();
    }

    void AnimSetToIdel()
    {
        animator.SetBool("isFinished", false);
        animator.SetBool("isIdle", true);
    }

    #endregion



    #region UI

    void UpdateImages()
    {
        if (machineType == TypeOfMachine.DryingMachine) return;
        if (hasDetergent)
        {
            detergentColour.color = hasObjectColour;
        }
        else
        {
            detergentColour.color = doesntHaveObjectColour;
        }
        if (hasFabricSoftener)
        {
            fabricSoftenerColour.color = hasObjectColour;
        }else
        {
            fabricSoftenerColour.color = doesntHaveObjectColour;
        }
        if (hasBasket)
        {
            basketColour.color = hasObjectColour;
        }else
        {
            basketColour.color = doesntHaveObjectColour;
        }
    }

    #endregion
}

public enum TypeOfMachine
{
    None, LaundryMachine, DryingMachine
}
