using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using FMODUnity;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IMachineInteractable
{
    [Title("Machine Info", null, TitleAlignments.Centered)]
    [SerializeField] MachineType machineType;
    [SerializeField] GameObject deliveryArea;
    [SerializeField] Transform spawnPos;
    [SerializeField] int maxAmountOfDryers;
    [SerializeField] int maxAmountOfWashingMachines;


    [Title("Moving Camera Variables", null, TitleAlignments.Centered)]
    [SerializeField] List<Transform> camPositions = new();
    [SerializeField] List<Transform> camLookAtPositions = new();
    //[SerializeField] GameObject buttonCanvas;
    [SerializeField] float camMoveTime;


    [Title("UI", null, TitleAlignments.Centered)]
    [SerializeField] GameObject tipCanvas;

    [Title("Sounds", null, TitleAlignments.Centered)]
    [SerializeField] StudioEventEmitter CamMoveSound;
    [SerializeField] StudioEventEmitter buySound;
    [SerializeField] StudioEventEmitter cantMoveSound;
    [SerializeField] StudioEventEmitter cantBuySound;


    [Title("Objects to Buy", "This is based on rows from top to bottom", TitleAlignments.Centered)]
    [SerializeField] List<GameObject> itemsToBuy = new();


    [Title("Prices", "This is based on rows from top to bottom", TitleAlignments.Centered)]
    [SerializeField] List<int> pricesList = new();
    [SerializeField] TextMeshProUGUI upgradePriceText;
    [SerializeField] TextMeshProUGUI detergentPriceText;
    [SerializeField] TextMeshProUGUI softenerPriceText;
    [SerializeField] TextMeshProUGUI laundryMachinePriceText;
    [SerializeField] TextMeshProUGUI dryingMachinePriceText;
    [SerializeField] Color inStockColour;
    [SerializeField] Color outOfStockColour;

    [Title("Upgrades")]
    [SerializeField] MachineManager machineMan;



    [Title("Washer and Dryers", null, TitleAlignments.Centered)]
    [SerializeField] List<GameObject> washingMashineList = new();
    [SerializeField] List<GameObject> dryerMachineList = new();

    PlayerInteraction playerInteraction;
    PlayerMovement playerMovement;
    InputSystem_Actions vendingActions;
    Vector3 camOriginalPosition;
    Quaternion originalCamRotation;
    GameObject currentCam;
    int currentCamInt;
    int deliveryAreaMaxAmount;
    bool isBusy;
    int amountOfDryersBought;
    int amountOfWashingMachinesBought;

    void Start()
    {
        vendingActions = new();
        deliveryAreaMaxAmount = deliveryArea.transform.childCount;
        UpdatePrices();

    }
    public void Interact(Camera camObject)
    {
        
        currentCam = camObject.gameObject;
        playerInteraction = camObject.transform.parent.parent.GetComponent<PlayerInteraction>();
        playerMovement = camObject.transform.parent.parent.GetComponent<PlayerMovement>();
        playerMovement.ResetCamMovement();
        playerMovement.cameraOriginalRotation = Camera.main.transform.rotation;
        playerMovement.cameraOriginalTransform = Camera.main.transform.position;
        playerInteraction.StopAndStartPlayerMovement(false);
        playerInteraction.StopAndStartPlayerInteraction(false);
        playerInteraction.canInteract = false;
        playerMovement.canMoveAndLook = false;
        

        camObject.transform.DOMove(camPositions[0].position, camMoveTime);
        camObject.transform.DODynamicLookAt(camLookAtPositions[0].position, camMoveTime).OnComplete(StartVendingMechanics);
        currentCamInt = 0;
    }

    void StartVendingMechanics()
    {
        //Enable new interactions
        vendingActions.Enable();
        tipCanvas.SetActive(true);
        #region Interactions

        vendingActions.VendingMachine.Up.performed += x => OnUp();
        vendingActions.VendingMachine.Down.performed += x => OnDown();
        vendingActions.VendingMachine.E.performed += x => OnEnterPressed();
        vendingActions.VendingMachine.Exit.performed += x => OnExitPressed();

        #endregion
        //Disable Player Movement
        
    }

    #region  Actions

    void OnUp()
    {

        if (currentCamInt <= 0)
        {
            cantMoveSound.Play();
            return;
        } 
        CamMoveSound.Play();
        currentCamInt--;
        currentCam.transform.DOMove(camPositions[currentCamInt].position, camMoveTime);

        //moveCamUpIfAble
    }

    void OnDown()
    {

        if (currentCamInt == camPositions.Count - 1)
        {
            cantMoveSound.Play();
            return;
        }
        CamMoveSound.Play();
        currentCamInt++;
        currentCam.transform.DOMove(camPositions[currentCamInt].position, camMoveTime);
        //moveCamDownIfAble
    }

    void OnEnterPressed()
    {
        //Ask to buy
        TryToBuyItem();
    }

    void OnExitPressed()
    {
        ExitMachine();
    }

    #endregion

    void UpdatePrices()
    {
        upgradePriceText.text = $"${pricesList[0].ToString()}";
        detergentPriceText.text = $"${pricesList[1].ToString()}";
        softenerPriceText.text = $"${pricesList[2].ToString()}";
        laundryMachinePriceText.text = $"${pricesList[3].ToString()}";
        dryingMachinePriceText.text = $"${pricesList[4].ToString()}";
    }
    public void ExitMachine()
    {
        tipCanvas.SetActive(false);
        vendingActions.Disable();
        playerInteraction.StopAndStartPlayerInteraction(true);
        playerMovement.ResetCamera();
        playerMovement.canMoveAndLook = true;
        SetCursorOff();
        
    }

    public bool IsBusy()
    {
        return isBusy;
    }

    public MachineType MachineType()
    {
        return machineType;
    }

    public void TryToBuyItem()
    {
        int price = GetLevelPrice(currentCamInt);
        if (PlayerSettings.instance.playerMoney >= price)
        {
            if (currentCamInt < 3)
            {
                if (currentCamInt != 0)
                {
                    PlayerSettings.instance.ReduceMoney(price);
                    SpawnSmallItem();
                    buySound.Play();
                }else if(currentCamInt == 0)
                {
                    PlayerSettings.instance.ReduceMoney(price);
                    BuyBigItem();
                    buySound.Play();
                }
            }
            else if(currentCamInt >= 3)
            {
                //Washing Machine
                if (currentCamInt == 3)
                {
                    if (amountOfWashingMachinesBought != maxAmountOfWashingMachines)
                    {
                        PlayerSettings.instance.ReduceMoney(price);
                        BuyBigItem();
                        buySound.Play();
                        ++amountOfWashingMachinesBought;
                        if(amountOfWashingMachinesBought >= maxAmountOfWashingMachines)
                        {
                            laundryMachinePriceText.color = outOfStockColour;
                            laundryMachinePriceText.text = "Out Of Stock";
                            playerInteraction.UpdatePlayerPopupMessage("Max amount of Washing Machines Bought");
                        }
                    }
                    else
                    {
                        cantBuySound.Play();
                        laundryMachinePriceText.color = outOfStockColour;
                    }
                }
                //Dryers
                if (currentCamInt == 4)
                {
                    if (amountOfDryersBought != maxAmountOfDryers)
                    {
                        PlayerSettings.instance.ReduceMoney(price);
                        BuyBigItem();
                        buySound.Play();
                        ++amountOfDryersBought;
                        if(amountOfDryersBought >= maxAmountOfDryers)
                        {
                            dryingMachinePriceText.color = outOfStockColour;
                            dryingMachinePriceText.text = "Out Of Stock";
                            playerInteraction.UpdatePlayerPopupMessage("Max amount of Drying Machines Bought");
                        }
                    }
                    else
                    {
                        cantBuySound.Play();
                    }
                }
            }

            //buy item
        }
        else
        {
            cantBuySound.Play();
        }
    }

    void BuySmallItem()
    {
        //Check waht item to buy
        //Place it on table
        int nextAvailableSpaceIndex = NextAvailableSpaceOnTable();
        if (nextAvailableSpaceIndex == 555)
        {
            Debug.Log("You fucked up theres no space");
            return;
        }
        Instantiate(itemsToBuy[currentCamInt], deliveryArea.transform.GetChild(nextAvailableSpaceIndex));
        //OnExitPressed();

    }

    void SpawnSmallItem()
    {
        GameObject currentObject = Instantiate(itemsToBuy[currentCamInt], spawnPos);
        Rigidbody rigidbody = currentObject.GetComponent<Rigidbody>();
        Collider collider = currentObject.GetComponent<Collider>();

        collider.enabled = true;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.freezeRotation = false;
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    void BuyBigItem()
    {
        if (currentCamInt == 0)
        {
            foreach (TimerMachines machineToUpgrade in machineMan.machineList)
            {
                machineToUpgrade.skipRythm = true;
            }
            upgradePriceText.color = outOfStockColour;
            upgradePriceText.text = $"Out Of Stock";
        }
        if (currentCamInt == 3)
        {
            washingMashineList[amountOfWashingMachinesBought].SetActive(true);
            //OnExitPressed();
        }
        else if (currentCamInt == 4)
        {
            dryerMachineList[amountOfDryersBought].SetActive(true);
            //OnExitPressed();
        }

    }
    

    int SpacesLeftOnTable()
    {
        int amountOfSpace = 0;
        for (int i = 0; i < --deliveryAreaMaxAmount; i++)
        {
            if (deliveryArea.transform.GetChild(i).childCount == 0)
            {
                amountOfSpace++;
            }

        }
        return amountOfSpace;
    }

    int NextAvailableSpaceOnTable()
    {
        for (int i = 0; i < deliveryAreaMaxAmount; i++)
        {
            if (deliveryArea.transform.GetChild(i).childCount == 0)
            {
                Debug.Log($"[Vending Machine] Next available space index {i}");
                return i;
            }
        }
        return 555;
    }

    int GetLevelPrice(int currentLevelIndex)
    {
        switch (currentLevelIndex)
        {
            case 0:
                return pricesList[0];
            case 1:
                return pricesList[1]; 
            case 2:
                return pricesList[2];
            case 3:
                return pricesList[3];
            case 4:
                return pricesList[4];
        }

        return 0;
    }

    public void SetCursorOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableActions()
    {
        vendingActions.Enable();
    }

    #region Unused

    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
