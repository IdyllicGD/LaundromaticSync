using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character_Machine_Interaction : MonoBehaviour
{
    [SerializeField] LayerMask machineInteractionLayer;
    [SerializeField] float raycastDistance;

    private Vector3 currentEulerAngles;
    
    public MachineFocusMode_Actions machineActions;
    private Machine_Interaction_Objects machineInteaction;
    
    private Character_Controller_Player characterControllerPlayer;
    private PlayerSettings playerSettings;
    private Camera playerCam;
    private Transform objectHit;
    
    private bool canMove;
    private bool canRotateObject;
    private bool movingObject = false;
    
    
    Transform player_Cam_Original_Transform;
    
    private float mouseSensitivity;
    private float mouseX;
    private float mouseY;
    private float rotationAmount;
    private float rotationAmountDial;

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
                    machineInteaction = hit.transform.GetComponent<Machine_Interaction_Objects>();
                    objectHit = hit.transform;
                    canRotateObject = true;
                }
                
                if (canRotateObject == false) return;
                switch (machineInteaction.Machine_Interactible_Type)
                {
                    case Machine_Interactible_Types.Dial:
                        LaundryDialMove(canRotateObject, objectHit, ray, mouseX, mouseY);
                        break;
                    case Machine_Interactible_Types.Lever:
                        LaundryLeverMove(canRotateObject, objectHit, ray, mouseX, mouseY);
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
        machineInteaction = null;
        objectHit = null;
    }
    
    
    #region Laundry Objects

    private void LaundryDialMove(bool canRotateObject, Transform objectHit, Ray ray, float mouseX, float mouseY)
    {
        //Debug.Log($"[{objectHit.localRotation.y}]");
        // if (objectHit.localRotation.y is > 0f and < 35f)
        // {
        //      0.22
        //      0.5
        //      0.7
        // }
        
        if (!canRotateObject) return;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        rotationAmountDial = Mathf.Clamp(rotationAmountDial,35f , 303f);
        rotationAmountDial -= mouseY;
        rotationAmountDial -= mouseX;
        objectHit.transform.localRotation = Quaternion.Euler(rotationAmountDial,0f ,-90f );
        // objectHit.transform.Rotate(0f, rotationAmountDial * mouseSensitivity * Time.deltaTime, 0f, Space.Self);
    }

    private void LaundryLeverMove(bool canMoveLever, Transform objectHit, Ray ray, float mouseX, float mouseY)
    {

        Debug.Log($"{objectHit.parent.localRotation.z}");
        if (objectHit.parent.localRotation.z <= -0.64f)
        {
            Debug.Log("Yippee lever puller");
            objectHit.parent.DORotate(new Vector3(0f, 0f, 10f), 1f);
            ExitFocusMode();
            rotationAmount = 0f;
            machineInteaction = null;
            objectHit = null;
            canMoveLever = false;
            canRotateObject = false;
        }
        
        if (!canMoveLever) return;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        rotationAmount = Mathf.Clamp(rotationAmount, -10, 80);
        rotationAmount -= mouseY;
        
        objectHit.parent.transform.localRotation = Quaternion.Euler(0f,0f , -rotationAmount);
    }
        
    #endregion
    
    
    void ExitFocusMode()
    {
        canMove = characterControllerPlayer.canMove;
        if (canMove) return;
        machineActions.Disable();
        Camera.main.transform.DOMove(characterControllerPlayer.player_Cam_Original_Transform, 0.5f);
        StartCoroutine(TimeToWait(0.6f));
        IEnumerator TimeToWait(float time)
        {
            float timeWaited = 0;
            timeWaited += Time.deltaTime;
            yield return new WaitForSeconds(time);
            Debug.Log($"[Time Waited {timeWaited}ms]]");
            characterControllerPlayer.canMove = true;
            characterControllerPlayer.playerActions.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            
        }
    }
    

    private void OnDestroy()
    {
        machineActions.Disable();
    }
}
