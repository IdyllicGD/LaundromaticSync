using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Character_Controller_Player : MonoBehaviour
{
    public InputSystem_Actions playerActions;
    public Vector3 player_Cam_Original_Transform;
    [HideInInspector]public bool canMove;
    
    
    PlayerSettings playerSettings;
    MachineFocusMode_Actions machineActions;
    Rigidbody playerRb;
    Vector3 moveDirection;
    Vector2 moveInput, lookInput, macineLookInput;
    float rotateX, rotateY;
    float mouseSensitivity;
    Camera playerCam;
    bool isLookingAtObject;
    bool isHoldingObject = false;
    public Transform handPosition;
    
    //Serialize fields
    [SerializeField] private Character_Machine_Interaction characterMachineInteraction;
    [SerializeField] private float playerSpeed, minClampDistance, maxClampDistance;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private LayerMask machineInteractionLayer;
    [SerializeField] private LayerMask registerLayer;
    [SerializeField] private float raycastDistance;
    [SerializeField] private int score;
    

    void FixedUpdate () {
        PlayerMovement ();
    }
    
    void Awake () {
        
        #region Start GetComponents

        playerRb = GetComponent<Rigidbody>();
        characterMachineInteraction = GetComponent<Character_Machine_Interaction> ();
        playerSettings = GetComponent<PlayerSettings> ();
        handPosition = transform.GetChild (0);

        #endregion

        #region Start Set Variables

        Cursor.lockState = CursorLockMode.Locked;
        playerCam = Camera.main;
        
        #endregion
        
        #region Set Player Settings

        mouseSensitivity = playerSettings.mouseSensitivity;

        #endregion
        
        //Player Inputs
        #region Actions
        
        playerActions = new InputSystem_Actions ();
        playerActions.Enable();
        playerActions.Player.Move.performed += OnMove;
        playerActions.Player.Move.canceled += OnMove;
        playerActions.Player.Look.performed += OnLook;
        playerActions.Player.Look.canceled += OnLook;
        playerActions.Player.Look.performed += x => MouseLookAround();
        playerActions.Player.Interact.performed += x => PlayerInteract ();
        
        #endregion
        
    }
    
    
    #region Controller
    

    public void OnMove (InputAction.CallbackContext obj) {
        moveInput = obj.ReadValue<Vector2> ();
        
    }

    public void OnLook (InputAction.CallbackContext obj) {
        lookInput = obj.ReadValue<Vector2> ();
    }

    void PlayerMovement () {
        if(!canMove) return;
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        moveDirection = camForward * moveInput.y + camRight * moveInput.x;
        playerRb.linearVelocity =  moveDirection * (playerSpeed * Time.fixedDeltaTime);

    }
    
    public void MouseLookAround () {
        if(!canMove) return;
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        rotateX = Mathf.Clamp(rotateX, minClampDistance, maxClampDistance);
        rotateX -= mouseY;
        
        playerCam.transform.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
        transform.Rotate (Vector3.up * mouseX);
        
    }

    #endregion

    #region Interactions

    #region Player Move Interactions
    void PlayerInteract () {
        if(!canMove) return;
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance,
                interactionLayer))
        {
            //Interact with Machine
            if (hit.transform.TryGetComponent(out Machine_Interaction interaction))
            {
                if (interaction.isMachineBusy) return;
                if (interaction.isCounterTop)
                {
                    if (handPosition.childCount <= 0) return;
                    if (handPosition.GetComponentInChildren<PickUpObject>().PickUpObjectType == PickUpObjectType.LaundryBasket)
                    {
                        Transform _pickUpObject = handPosition.GetChild(0);
                        _pickUpObject.DOMove(interaction.table_Top_Position.position, 1f);
                        _pickUpObject.SetParent(interaction.transform);
                        return;
                    }
                
                    //place objects
                    //return
                }
                InteractWithMachine(interaction);
                return;
            }
            //Pick up object
            if (hit.transform.TryGetComponent(out PickUpObject objectToPickUp))
            {
                if (objectToPickUp.PickUpObjectType == PickUpObjectType.LaundryBasket && characterMachineInteraction!= null)
                {
                    characterMachineInteraction.hasBasket = true;
                    characterMachineInteraction.basket = objectToPickUp.gameObject;
                }
                PickupObject(objectToPickUp.gameObject);
            
                return;
            }
        }
        

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, registerLayer))
        {
            Debug.Log("Hit");
            if (handPosition.childCount > 0)
            {
                
                handPosition.GetChild(0).TryGetComponent(out PickUpObject _pickUpObject);
                if (_pickUpObject.IsClean)
                {
                    score++;
                    Destroy(handPosition.GetChild(0).gameObject);
                }
            }
        }
    }


    void InteractWithMachine(Machine_Interaction interaction)
    {
        EnterFocusMode(interaction.machine_Cam_Transform, interaction.look_At_Target, interaction.basket_Position, interaction);
        //interaction.OpenDoorOnStart();
    }

    void PickupObject(GameObject objectToPickup)
    {
        objectToPickup.transform.DOMove(handPosition.position, 0.1f).WaitForCompletion();
        objectToPickup.transform.SetParent(handPosition);
        isHoldingObject = true;
    }
    #endregion
    
    #region Focus Modes
    void EnterFocusMode(Transform camera_Move_Position, Transform look_At_Target, Transform basket_Position, Machine_Interaction interaction)
    {
        characterMachineInteraction.machineInteractionParent = interaction;
        playerActions.Disable();
        canMove = false;
        player_Cam_Original_Transform = Camera.main.transform.position;
        Camera.main.transform.DOMove(camera_Move_Position.position, 1f);
        if (characterMachineInteraction.hasBasket)
        {
            characterMachineInteraction.basket.transform.DOMove(basket_Position.position, 1f);
        }
        
        StartCoroutine(TimeToComplete(1.1f));
        
        IEnumerator TimeToComplete(float time)
        {
            yield return new WaitForSeconds(time);
            Camera.main.transform.DOLookAt(look_At_Target.transform.position, 0.5f);
            characterMachineInteraction.machineActions.Enable();
            Cursor.lockState = CursorLockMode.None;
        }
        
    }
    #endregion

    #endregion
    
    private void OnDestroy()
    {
        playerActions.Disable();
    }
    
}


