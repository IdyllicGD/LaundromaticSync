using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class VendingMachine : MonoBehaviour, IMachineInteractable
{
    [Title("Machine Info", null, TitleAlignments.Centered)]
    [SerializeField] MachineType machineType;
    [SerializeField] GameObject deliveryArea;
    [SerializeField] int maxAmountOfDryers;
    [SerializeField] int maxAmountOfWashingMachines;


    [Title("Moving Camera Variables", null, TitleAlignments.Centered)]
    [SerializeField] List<Transform> camPositions = new();
    [SerializeField] List<Transform> camLookAtPositions = new();
    //[SerializeField] GameObject buttonCanvas;
    [SerializeField] float camMoveTime;


    [Title("UI", null, TitleAlignments.Centered)]
    [SerializeField] GameObject buyCanvas;


    [Title("Sounds", null, TitleAlignments.Centered)]
    [SerializeField] StudioEventEmitter CamMoveSound;
    [SerializeField] StudioEventEmitter buySound;
    [SerializeField] StudioEventEmitter cantMoveSound;
    [SerializeField] StudioEventEmitter cantBuySound;


    [Title("Objects to Buy", "This is based on rows from top to bottom", TitleAlignments.Centered)]
    [SerializeField] List<GameObject> itemsToBuy = new();


    [Title("Prices", "This is based on rows from top to bottom", TitleAlignments.Centered)]
    [SerializeField] List<int> pricesList = new();


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
    }
    public void Interact(Camera camObject)
    {
        currentCam = camObject.gameObject;
        playerInteraction = camObject.transform.parent.parent.GetComponent<PlayerInteraction>();
        playerMovement = camObject.transform.parent.parent.GetComponent<PlayerMovement>();
        playerMovement.cameraOriginalRotation = Camera.main.transform.rotation;
        playerMovement.cameraOriginalTransform = Camera.main.transform.position;

        camObject.transform.DOLocalMove(camPositions[0].position, camMoveTime);
        camObject.transform.DODynamicLookAt(camLookAtPositions[0].position, camMoveTime).OnComplete(StartVendingMechanics);
        currentCamInt = 0;
    }

    void StartVendingMechanics()
    {
        //Enable new interactions
        vendingActions.Enable();
        #region Interactions

        vendingActions.VendingMachine.Up.performed += x => OnUp();
        vendingActions.VendingMachine.Down.performed += x => OnDown();
        vendingActions.VendingMachine.Enter.performed += x => OnEnterPressed();

        #endregion
        //Disable Player Movement
        playerInteraction.StopAndStartPlayerMovement();
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
        currentCam.transform.DOLocalMove(camPositions[currentCamInt].position, camMoveTime);

        //moveCamUpIfAble
    }

    void OnDown()
    {

        if (currentCamInt >= camPositions.Count)
        {
            cantMoveSound.Play();
            return;
        }
        CamMoveSound.Play();
        currentCamInt++;
        currentCam.transform.DOLocalMove(camPositions[currentCamInt].position, camMoveTime);
        //moveCamDownIfAble
    }

    void OnEnterPressed()
    {
        //Ask to buy
        buyCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

    public void ExitMachine()
    {
        playerMovement.ResetCamera();

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
            if (currentCamInt < 3 && SpacesLeftOnTable() != 0)
            {
                PlayerSettings.instance.ReduceMoney(price);
                BuySmallItem();
            }
            else if(currentCamInt <= 3 && SpacesLeftOnTable() != 0)
            {
                //Washing Machine
                if (currentCamInt == 3)
                {
                    if (amountOfWashingMachinesBought != maxAmountOfWashingMachines)
                    {
                        PlayerSettings.instance.ReduceMoney(price);
                        BuyBigItem();
                    }
                    else
                    {
                        cantBuySound.Play();
                        playerInteraction.UpdatePlayerPopupMessage("Max amount of Washing Machines Bought");
                    }
                }
                //Dryers
                else if (currentCamInt == 4)
                {
                    if (amountOfDryersBought != maxAmountOfDryers)
                    {
                        PlayerSettings.instance.ReduceMoney(price);
                        BuyBigItem();
                    }
                    else
                    {
                        cantBuySound.Play();
                        playerInteraction.UpdatePlayerPopupMessage("Max amount of Drying Machines Bought");
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

    }

    void BuyBigItem()
    {
        if (currentCamInt == 3)
        {
            washingMashineList[amountOfWashingMachinesBought].SetActive(true);
        }
        else if (currentCamInt == 4)
        {
            dryerMachineList[amountOfDryersBought].SetActive(true);
        }
        
    }

    int SpacesLeftOnTable()
    {
        int amountOfSpace = 0;
        for (int i = 0; i < deliveryAreaMaxAmount; i++)
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

    #region Unused

    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
