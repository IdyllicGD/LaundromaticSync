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
    [SerializeField] StudioEventEmitter shopMusic;
    [SerializeField] StudioEventEmitter mainMusic;


    [Title("Objects to Buy", "This is based on rows from top to bottom", TitleAlignments.Centered)]
    [SerializeField] List<GameObject> itemsToBuy = new();


    [Title("Prices", "This is based on rows from top to bottom", TitleAlignments.Centered)]
    [SerializeField] List<int> pricesList = new();
    [SerializeField] TextMeshProUGUI upgradePriceText;
    [SerializeField] TextMeshProUGUI detergentPriceText;
    [SerializeField] TextMeshProUGUI softenerPriceText;
    [SerializeField] TextMeshProUGUI laundryMachinePriceText;
    [SerializeField] TextMeshProUGUI dryingMachinePriceText;
    [SerializeField] TextMeshProUGUI currentUpgradeTextInfo;
    [SerializeField] TextMeshProUGUI currentUpgradeText;
    [SerializeField] Color inStockColour;
    [SerializeField] Color outOfStockColour;

    [Title("Upgrades")]
    [SerializeField] MachineManager machineMan;
    [SerializeField] Upgrades currentSelectedUpgrade;
    [SerializeField] List<UpgradeInfo> upgradeInfo = new();
    [SerializeField] GameObject popupInfoTextGameObject;

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
        UpdateUpgradeText();


    }
    public void Interact(Camera camObject)
    {
        mainMusic.Stop();
        shopMusic.Play();
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
        popupInfoTextGameObject.SetActive(true);
        #region Interactions

        vendingActions.VendingMachine.Up.performed += x => OnUp();
        vendingActions.VendingMachine.Down.performed += x => OnDown();
        vendingActions.VendingMachine.E.performed += x => OnEnterPressed();
        vendingActions.VendingMachine.Exit.performed += x => OnExitPressed();
        vendingActions.VendingMachine.Left.performed += x => OnLeft();
        vendingActions.VendingMachine.Right.performed += x => OnRight();

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
        if (currentCamInt == 1)
        {
            UpdateUpgradeText();
            popupInfoTextGameObject.SetActive(true);
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
        if (currentCamInt == 0)
        {
            popupInfoTextGameObject.SetActive(false);
        }
        CamMoveSound.Play();
        currentCamInt++;
        currentCam.transform.DOMove(camPositions[currentCamInt].position, camMoveTime);
        //moveCamDownIfAble
    }

    void OnLeft()
    {
        if (currentCamInt != 0) return;
        if (currentSelectedUpgrade == 0)
        {
            cantMoveSound.Play();
            return;
        }
        --currentSelectedUpgrade;
        UpdateUpgradeText();

    }

    void OnRight()
    {
        if (currentCamInt != 0) return;
        if ((int)currentSelectedUpgrade == 5)
        {
            cantMoveSound.Play();
            return;
        }
        ++currentSelectedUpgrade;
        UpdateUpgradeText();
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
        mainMusic.Play();
        shopMusic.Stop();
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
            if (currentCamInt != 0)
            {
                PlayerSettings.instance.ReduceMoney(price);
                SpawnItem();
                buySound.Play();
            }
            else if (currentCamInt == 0)
            {
                BuyUpgrades();
            }
        }
        else
        {
            cantBuySound.Play();
        }
    }

    void SpawnItem()
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


    #region Upgrades
    void BuyUpgrades()
    {
        if (PlayerSettings.instance.playerMoney < upgradeInfo[(int)currentSelectedUpgrade].price)
        {
            cantBuySound.Play();
            playerInteraction.UpdatePlayerPopupMessage("Sorry, you're too broke for this!");
            return;
        }
        if (upgradeInfo[(int)currentSelectedUpgrade].owned)
        {
            cantBuySound.Play();
            playerInteraction.UpdatePlayerPopupMessage("You already own this buddy... You feeling okay?");
            return;
        }
        buySound.Play();
        int currentUpgradeIndex = (int)currentSelectedUpgrade;
        PlayerSettings.instance.ReduceMoney(upgradeInfo[currentUpgradeIndex].price);
        switch (currentSelectedUpgrade)
        {

            case Upgrades.NoQte:
                foreach (TimerMachines machineToUpgrade in machineMan.machineList)
                {
                    machineToUpgrade.skipRythm = true;
                }
                upgradeInfo[currentUpgradeIndex].owned = true;
                UpdateUpgradeText();
                break;
            case Upgrades.SpeedBoost:
                Debug.Log($"[Vending Machine] {currentSelectedUpgrade} bought");
                playerMovement.playerOriginalSpeed *= 1.5f;
                playerMovement.playerSpeed *= 1.5f;
                upgradeInfo[currentUpgradeIndex].owned = true;
                UpdateUpgradeText();
                break;
            case Upgrades.JumpBoost:
                Debug.Log($"[Vending Machine] {currentSelectedUpgrade} bought");
                playerMovement.jumpForce = 550f;
                upgradeInfo[currentUpgradeIndex].owned = true;
                UpdateUpgradeText();
                break;
            case Upgrades.ProfitBoost:
                Debug.Log($"[Vending Machine] {currentSelectedUpgrade} bought");
                upgradeInfo[currentUpgradeIndex].owned = true;
                playerInteraction.upgradeMoneyMultiplier = 2;
                UpdateUpgradeText();
                break;
            case Upgrades.ThrowBoost:
                Debug.Log($"[Vending Machine] {currentSelectedUpgrade} bought");
                upgradeInfo[currentUpgradeIndex].owned = true;
                playerInteraction.maxForceAmount *= 2f;
                UpdateUpgradeText();
                break;
            case Upgrades.ThrowChargeShorter:
                Debug.Log($"[Vending Machine] {currentSelectedUpgrade} bought");
                upgradeInfo[currentUpgradeIndex].owned = true;
                playerInteraction.hasUpgrade = true;
                playerInteraction.maxHoldTime /= 2;
                UpdateUpgradeText();
                break;
        }
        

    }

    void UpdateUpgradeText()
    {
        if (upgradeInfo[(int)currentSelectedUpgrade].owned)
        {
            upgradePriceText.text = "OUT OF STOCK";
            currentUpgradeText.text = upgradeInfo[(int)currentSelectedUpgrade].upgradeName;
            upgradePriceText.color = outOfStockColour;
            currentUpgradeTextInfo.text = upgradeInfo[(int)currentSelectedUpgrade].upgradePopupInfo;
        }
        else
        {
            currentUpgradeText.text = upgradeInfo[(int)currentSelectedUpgrade].upgradeName;
            upgradePriceText.text = $"${upgradeInfo[(int)currentSelectedUpgrade].price.ToString()}";
            currentUpgradeTextInfo.text = upgradeInfo[(int)currentSelectedUpgrade].upgradePopupInfo;
            upgradePriceText.color = inStockColour;
        }
    }

#endregion


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

    public void Interact(Camera camObject, GameObject secondaryObject, bool i)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}

public enum Upgrades
{
    NoQte, SpeedBoost, JumpBoost, ProfitBoost, ThrowBoost, ThrowChargeShorter
}

[System.Serializable]
public class UpgradeInfo
{
    public string upgradeName;
    public string upgradePopupInfo;
    public int price;
    public bool owned;
}
