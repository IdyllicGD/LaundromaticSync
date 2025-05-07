using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character_Machine_Interaction : MonoBehaviour
{
    [SerializeField] LayerMask machineInteractionLayer;
    [SerializeField] float raycastDistance;

    //encapsulate data
    [SerializeField] DialSettings dialSettings;
    public DialSettings DialSettings => dialSettings;
    
    private Vector3 currentEulerAngles;
    
    public MachineFocusMode_Actions machineActions;
    private Machine_Interaction_Objects machineInteraction;
    
    private Character_Controller_Player characterControllerPlayer;
    private PlayerSettings playerSettings;
    private Camera playerCam;
    private Transform objectHit;
    
    private bool isDoorClosed = true;
    private bool canMove;
    private bool canRotateObject;
    private bool movingObject = false;
    
    
    Transform player_Cam_Original_Transform;
    
    private float mouseSensitivity;
    private float mouseX;
    private float mouseY;
    private float rotationAmount;
    private float rotationAmountDial;
    private float rotationAmountDoor;
    private float machineTimer;

    private Vector2 machineLookInput;

    private void Awake()
    {

        #region Start GetComponents
        playerCam = Camera.main;
        characterControllerPlayer = GetComponent<Character_Controller_Player>();
        playerSettings = GetComponentInParent<PlayerSettings>();
        
        #endregion
        
        #region Actions

        machineActions = new MachineFocusMode_Actions();
        machineActions.MachineInteractions.ExtFocusMode.performed += x => ExitFocusMode();
        machineActions.MachineInteractions.MouseLook.performed += MachineMousePosition;
        machineActions.MachineInteractions.MouseLook.canceled += MachineMousePosition;
        machineActions.MachineInteractions.MouseExit.performed += x => MouseExit();
        

        #endregion
        
        #region Set Player Settings
        
        mouseSensitivity = playerSettings.mouseSensitivity;
        
        #endregion
    }

    #region Mouse
    
    void MachineMousePosition(InputAction.CallbackContext obj)
    {
        machineLookInput = obj.ReadValue<Vector2>();
        if (machineActions.MachineInteractions.MouseInteraction.inProgress )
        {
            MouseInteraction();
            //canRotateObject = true;
        }
    }
    
    
    private void MouseInteraction()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = default;
        bool wasHit = false;
        float mouseX = machineLookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = machineLookInput.y * mouseSensitivity * Time.deltaTime;
        

        if (Physics.Raycast(ray, out hit, 500f, machineInteractionLayer) || canRotateObject)
        {
                if (canRotateObject == false)
                {
                    machineInteraction = hit.transform.GetComponent<Machine_Interaction_Objects>();
                    objectHit = hit.transform;
                    canRotateObject = true;
                }
                
                if (canRotateObject == false) return;
                switch (machineInteraction.Machine_Interactible_Type)
                {
                    case Machine_Interactible_Types.Dial:
                        LaundryDialMove(canRotateObject, objectHit, ray, mouseX, mouseY);
                        break;
                    case Machine_Interactible_Types.Lever:
                        LaundryLeverMove(canRotateObject, objectHit, ray, mouseX, mouseY);
                        break;
                    case Machine_Interactible_Types.Door:
                        LaundryDoorMove(canRotateObject, objectHit, ray, mouseX, mouseY);
                        break;
                    case Machine_Interactible_Types.MachineBasket:
                        LaundryBasketFill(objectHit, ray);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            
        }
        
    }

    void MouseExit()
    {
        Debug.Log($"Released");
        canRotateObject = false;
        machineInteraction = null;
        objectHit = null;
    }
    
    #endregion
    
    #region Laundry Objects

    private void LaundryDialMove(bool canRotateObject, Transform objectHit, Ray ray, float mouseX, float mouseY)
    {
        Debug.Log($"[{objectHit.transform.rotation.x}]");
        switch (objectHit.localRotation.x)
        {
            case >= 45f and < 90.01f:
                Debug.Log("[Option one]");
                break;
            case >= 90.01f and < 180f:
                Debug.Log("[Option Two]");
                break;
            case >= 180f and < 270f:
                Debug.Log("[Option Three]");
                break;
            case >= 270f and < 350f:
                Debug.Log("[Option Four]");
                break;
        }
        
        if (!canRotateObject) return;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        rotationAmountDial = Mathf.Clamp(rotationAmountDial,45f,330f);
        rotationAmountDial -= mouseY;
        rotationAmountDial -= mouseX;
        objectHit.transform.localRotation = Quaternion.Euler(rotationAmountDial,0f ,-90f );
        
    }

    private void LaundryLeverMove(bool canMoveLever, Transform objectHit, Ray ray, float mouseX, float mouseY)
    {
        if (objectHit.parent.localRotation.z <= -0.64f)
        {
            objectHit.parent.DORotate(new Vector3(0f, 0f, 10f), 1f);
            //check if door is closed
            if (!isDoorClosed) return;
            Debug.Log($"[Lever Pulled] Door is still open please close it before starting");
            //get setting
            switch (DialSettings)
            {
                case DialSettings.LowSetting:
                    machineTimer = 5f;
                    break;
                case DialSettings.MediumSetting:
                    machineTimer = 10f;
                    break;
                case DialSettings.HighSetting:
                    machineTimer = 20f;
                    break;
            }
            //Start machine
            
            objectHit.parent.parent.GetComponent<Machine_Interaction>().Interact();
            ExitFocusMode();
            rotationAmount = 0f;
            machineInteraction = null;
            objectHit = null;
            canMoveLever = false;
            canRotateObject = false;
            StartCoroutine(TimeToWait(machineTimer, "Start Lever Timer"));
        }
        
        if (!canMoveLever) return;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        rotationAmount = Mathf.Clamp(rotationAmount, -10, 80);
        rotationAmount -= mouseY;
        
        objectHit.parent.transform.localRotation = Quaternion.Euler(0f,0f , -rotationAmount);
    }

    private void LaundryDoorMove(bool canMoveDoor, Transform objectHit, Ray ray, float mouseX, float mouseY)
    {
        Debug.Log("Move door");
        
        if (!canMoveDoor) return;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        rotationAmountDoor = Mathf.Clamp(rotationAmountDoor,0f,150f);
        rotationAmountDoor -= mouseX;
        
        objectHit.parent.transform.localRotation = Quaternion.Euler(0f,rotationAmountDoor ,0f);
    }

    private void LaundryBasketFill(Transform objectHit, Ray ray)
    {
        
        objectHit.GetComponent<Material>().color = Color.black;
    }
        
    #endregion
    
    
    void ExitFocusMode()
    {
        
        canMove = characterControllerPlayer.canMove;
        if (canMove) return;
        machineActions.Disable();
        Camera.main.transform.DOMove(characterControllerPlayer.player_Cam_Original_Transform, 0.5f);
        
        StartCoroutine(TimeToWait(0.6f, "ExitFocusMode"));
        characterControllerPlayer.canMove = true;
        characterControllerPlayer.playerActions.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    
    #region Timer and OnDestroy
    //Timer
    IEnumerator TimeToWait(float time, string forWhat)
    {
        float timeWaited = 0;
        timeWaited += Time.deltaTime;
        yield return new WaitForSeconds(time);
        Debug.Log($"[Time Waited {timeWaited}ms] for {forWhat}]");
    }
    

    private void OnDestroy()
    {
        machineActions.Disable();
    }
    
    #endregion
}

public enum DialSettings
{
    LowSetting,MediumSetting,HighSetting
}
