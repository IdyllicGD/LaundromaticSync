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

    [Title("Animation", null, TitleAlignments.Centered)]
    [SerializeField] Animator animator;



    [Title("Colours on Machine", null, TitleAlignments.Centered)]
    [SerializeField] Color hasObjectColour;
    [SerializeField] Color doesntHaveObjectColour;
    [SerializeField] Image detergentColour, fabricSoftenerColour, basketColour;

    [Title("Canvasas")]
    public GameObject canvasTimer, canvasGroup;


    [SerializeField]GameObject currentBasket;
    bool isDone;
    public bool interupt;
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
        if (targetObject.TryGetComponent(out Basket basket))
        {
            if (basket.BasketType == correctBasket)
            {
                currentBasket = targetObject.gameObject;
                Interact(Camera.main, targetObject.gameObject);
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
                Interact(Camera.main, targetObject.gameObject);
            }else if(currentMachineUseableObject.ReturnMachineUsableItemsType() == MachineUsableItemsType.FabricSoftener)
            {
                currentFabricSoftener = targetObject.gameObject;
                Interact(Camera.main, targetObject.gameObject);
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

    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        if (isBusy) return;
        if (secondaryObject.TryGetComponent(out MachineUsableObjects itemType))
        {
            MachineUsableItemsType _type = itemType.ReturnMachineUsableItemsType();
            Debug.Log($"Current item type {_type}");
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
        Destroy(playerInteraction.handPosition.GetChild(0).gameObject);
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
            currentBasket = playerInteraction.handPosition.GetChild(0).gameObject;
        }
        GameObject playerHandObject = currentBasket;
        playerHandObject.transform.parent = endPostion;
        playerHandObject.transform.DOLocalMove(Vector3.zero, objectMoveTime);
        playerHandObject.transform.DOLocalRotate(Vector3.zero, objectMoveTime);
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
        if (timerTime >= maxMachineTimeToComplete) return;
        timerTime += timeToAddToTimer;
        TimeTextRythm.text = machineTimeToComplete.ToString();
    }

    #endregion

    #region Start Machine

    void StartMachine()
    {
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
        busySound.Stop();
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
    LaundryMachine, DryingMachine
}
