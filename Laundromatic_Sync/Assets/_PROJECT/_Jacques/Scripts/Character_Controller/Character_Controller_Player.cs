using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Character_Controller_Player : MonoBehaviour
{
    public InputSystem_Actions playerActions;
    public Vector3 player_Cam_Original_Transform;
    [HideInInspector]public bool canMove;
    
    Character_Machine_Interaction character_Machine_Interaction;
    
    PlayerSettings playerSettings;
    MachineFocusMode_Actions machineActions;
    Rigidbody playerRb;
    Vector3 moveDirection;
    Vector2 moveInput, lookInput, macineLookInput;
    float rotateX, rotateY;
    float mouseSensitivity;
    Camera playerCam;
    bool isLookingAtObject;
    
    
    //Serialize fields
    [SerializeField] private Character_Machine_Interaction characterMachineInteraction;
    [SerializeField] float playerSpeed, minClampDistance, maxClampDistance;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] private LayerMask machineInteractionLayer;
    [SerializeField] float raycastDistance;

    void FixedUpdate () {
        PlayerMovement ();
    }
    
    void Awake () {
        
        #region Start GetComponents

        playerRb = GetComponent<Rigidbody>();
        characterMachineInteraction = GetComponent<Character_Machine_Interaction> ();
        playerSettings = GetComponent<PlayerSettings> ();

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
        
        //Raycast hitting the interactable object, getting the script off of the object that derives from an interface
        //Runs the code on that script to start the mechanics
        
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, interactionLayer)) {
            Machine_Interaction _interactable_Machine = hit.collider.gameObject.GetComponent<Machine_Interaction> ();
            if (_interactable_Machine != null) { }
            EnterFocusMode(_interactable_Machine.machine_Cam_Transform, _interactable_Machine.look_At_Target);
            _interactable_Machine.Interact();
        }
    }
    #endregion
    
    #region Focus Modes
    void EnterFocusMode(Transform camera_Move_Position, Transform look_At_Target)
    {
        playerActions.Disable();
        canMove = false;
        player_Cam_Original_Transform = Camera.main.transform.position;
        Camera.main.transform.DOMove(camera_Move_Position.position, 1f);
        
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


