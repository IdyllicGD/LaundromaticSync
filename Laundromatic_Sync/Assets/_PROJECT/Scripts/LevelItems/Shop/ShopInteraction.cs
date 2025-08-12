using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;



public class ShopInteraction : MonoBehaviour, IMachineInteractable
{
    MachineType machineType = _PROJECT._Jacques.Scripts.Interfaces.MachineType.Shop;

    [Title("Camera Variables", null, TitleAlignments.Centered)]
    [SerializeField] Transform lookAtPosition;
    [SerializeField] Transform camPosition;
    [SerializeField] float baseSpeedCam;

    [Title("Ui Vars", null, TitleAlignments.Centered)]
    [SerializeField] LayerMask uiLayer;
    [Title("Look Around Vars", null, TitleAlignments.Centered)]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float minClampDistanceX;
    [SerializeField] float maxClampDistanceX;
    [SerializeField] float minClampDistanceY;
    [SerializeField] float maxClampDistanceY;
    Vector2 lookInput;
    bool canLookAround;
    float rotateX;
    float rotateY;
    Transform camOriginalPosition;
    Transform camOriginalLookAtPosition;
    Camera currentPlayerCam;
    PlayerInteraction playerInteraction;
    InputSystem_Actions inputActions;
    Quaternion camOriginalRotateAngle;
    Quaternion camLastRotateAngle;
    bool hasPreviouslyUsed;
    int newOrdersCount;


    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.Click.performed += x => MousePressed();
        inputActions.Player.Look.performed += LookAround;
    }

    void Update()
    {
        if (!canLookAround) return;
        CamLook();
    }

    void MousePressed()
    {   
        Debug.Log("[ShopInteraction] MousePressed");
        Debug.DrawRay(currentPlayerCam.transform.position, currentPlayerCam.transform.forward, Color.red);
        if (!Physics.Raycast(currentPlayerCam.transform.position, currentPlayerCam.transform.forward, out RaycastHit hit, uiLayer))
        {
            Debug.Log("[ShopInteraction] Nothing was Hit");
            return; }
        if (hit.transform.TryGetComponent(out ButtonInfoShop button))
        {
            if (button.price <= PlayerSettings.instance.playerMoney)
            {
                Debug.Log(DayNight.instance.ReturnAvailableSpaceOnDeliveryTable());
                if (DayNight.instance.ReturnAvailableSpaceOnDeliveryTable() - newOrdersCount <= 0)
                {
                    playerInteraction.UpdatePlayerPopupMessage("There is not enough space in the delivery area to order this item");
                    return;
                }
                else
                {
                    PlayerSettings.instance.ReduceMoney(button.price);
                    ShopManager.instance.OrderItem(button.orderItem);
                    newOrdersCount++;
                }
                
            }
            else
            {
                //play ERROR sound
                playerInteraction.UpdatePlayerPopupMessage("You are too broke");
            }
            
        }else if (hit.transform.TryGetComponent(out Button unityEvent))
        {
            unityEvent.onClick.Invoke();
        }


    }

    void LookAround(InputAction.CallbackContext obj)
    {
        lookInput = obj.ReadValue<Vector2>();
    }

    void CamLook()
    {
        if (!inputActions.Player.Look.inProgress) return;
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        rotateX = Mathf.Clamp(rotateX, minClampDistanceX, maxClampDistanceX);
        rotateX -= mouseY;
        rotateY = Mathf.Clamp(rotateY, minClampDistanceY, maxClampDistanceY);
        rotateY -= mouseX;
        
        currentPlayerCam.transform.localRotation = Quaternion.Euler(rotateX, -rotateY, 0f);
        currentPlayerCam.transform.Rotate (Vector3.up * mouseX);
    }


    public void Interact(Camera camObject)
    {
        lookInput = Vector2.zero;
        canLookAround = true;
        camOriginalRotateAngle = camObject.transform.rotation;
        playerInteraction = camObject.transform.parent.GetComponentInParent<PlayerInteraction>();
        playerInteraction.StopAndStartPlayerMovement();
        currentPlayerCam = camObject;

        //Get original cam position to move back to when leaving the shop
        camOriginalPosition = camObject.transform;
        //Raycast to get the position the player was looking at when interacting with the machine to use later to do a DODynamicLookAt on that point to make it more seemles when moving back from the cam position
        //DOTween time
        gameObject.GetComponent<Collider>().enabled = false;
        camObject.transform.DOMove(camPosition.position, baseSpeedCam);
        if (hasPreviouslyUsed)
        {
            camObject.transform.DORotateQuaternion(camLastRotateAngle, baseSpeedCam).OnComplete(() => SecondaryActions());
        }
        else
        {
            camObject.transform.DODynamicLookAt(lookAtPosition.position, baseSpeedCam).OnComplete(() => SecondaryActions());
        }
    }

    void SecondaryActions()
    {
        inputActions.Enable();
        
    }

    public void LeaveShop()
    {
        camLastRotateAngle = currentPlayerCam.transform.rotation;
        hasPreviouslyUsed = true;
        canLookAround = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.GetComponent<Collider>().enabled = true;
        currentPlayerCam.transform.DOLocalMove(Vector3.zero, baseSpeedCam);
        currentPlayerCam.transform.DORotateQuaternion(camOriginalRotateAngle, baseSpeedCam).OnComplete(() => playerInteraction.StopAndStartPlayerMovement());
    }

    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        //NOT USED
        Interact(camObject);
    }

    public bool IsBusy()
    {
        return false;
    }

    public MachineType MachineType()
    {
        return machineType;
    }
}
