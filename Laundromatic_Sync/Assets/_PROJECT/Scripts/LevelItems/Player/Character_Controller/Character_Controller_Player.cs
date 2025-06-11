using System.Collections;
using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Character_Controller_Player : MonoBehaviour
{
    public static Character_Controller_Player instance;
    
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
    private GameObject basket;
    public Transform handPosition;
    public float score;

    private float timer = 120f;
    
    [SerializeField]float controllerSensitivity;
    
    //Serialize fields
    [SerializeField] private Character_Machine_Interaction characterMachineInteraction;
    [SerializeField] private float playerSpeed, minClampDistance, maxClampDistance;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private LayerMask machineInteractionLayer;
    [SerializeField] private LayerMask tableLayer;
    [SerializeField] private LayerMask registerLayer;
    [SerializeField] private float raycastDistance;

    [SerializeField] private GameObject coinSound;

    [SerializeField] private GameObject PauseMenu;

    #region HUD Refs

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;

    #endregion
    

    void FixedUpdate () {
        PlayerMovement ();
        MouseLookAround();
        // timer -= Time.deltaTime;
        // timeText.text = Mathf.RoundToInt(timer).ToString();
        // if (timer <= 0)
        // {
        //     SceneManager.LoadScene(0);
        // }
        
    }
    
    void Awake () {
        
        instance = this;
        
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
        playerActions.Player.Exit_Focus_Mode.performed += x => PauseAndPlay();

        #endregion

    }

    void PauseAndPlay()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
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
        if (Gamepad.current != null)
        {
            mouseSensitivity = controllerSensitivity;
        }else if (Gamepad.current == null)
        {
            mouseSensitivity = playerSettings.mouseSensitivity;
        }
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

    void PlayerInteract()
    {
        Debug.Log("Interact");
        if (!canMove) return;
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance,
                interactionLayer))
        {
            if (hit.transform.TryGetComponent(out TableInteraction tableInteraction))
            {
                EnterFocusMode(tableInteraction.camPos, tableInteraction.camLookAtPos);
                tableInteraction._inputAction.Enable();
                tableInteraction.mainCanvas.gameObject.SetActive(true);
                tableInteraction.originalCamPos = playerCam.transform;
                Cursor.lockState = CursorLockMode.None;
            }
            //interacting with other object types
            if (hit.transform.TryGetComponent(out Iinteractable interactable))
            {
                interactable.Interact();
            }
            //Interact with Machine
            if (hit.transform.TryGetComponent(out Machine_Interaction interaction))
            {
                if (interaction.isMachineBusy) return;
                if (interaction.isCounterTop)
                {
                    if (handPosition.childCount <= 0) return;
                    if (handPosition.GetComponentInChildren<PickUpObject>().PickUpObjectType ==
                        PickUpObjectType.LaundryBasket)
                    {
                        Transform _pickUpObject = handPosition.GetChild(0);
                        _pickUpObject.DOMove(interaction.table_Top_Position.position, 1f);
                        _pickUpObject.SetParent(interaction.transform);
                        return;
                    }
                }
            }

            //Interactable Machine
            if (hit.transform.TryGetComponent(out I_Interactable_Machine interactableMachine))
            {
                if (transform.GetChild(0).childCount == 0)
                {
                    Debug.Log("[No Basket detected]");
                    return;

                }

                if (hit.transform.TryGetComponent(out LaundryMacineRhythm rhythmL))
                {
                    if (rhythmL.transform.GetChild(0).childCount != 0) return;
                    interactableMachine.Interact();
                    rhythmL.originalCamPosition = playerCam.transform.position;
                    EnterFocusMode(rhythmL.camPosition, rhythmL.lookAtPosition, rhythmL.basketPosition, rhythmL);
                }
            }

            //Pick up object
            if (hit.transform.TryGetComponent(out PickUpObject objectToPickUp))
            {
                if (handPosition.childCount != 0) return;
                if (objectToPickUp.PickUpObjectType == PickUpObjectType.LaundryBasket)
                {
                    basket = objectToPickUp.gameObject;
                }

                PickupObject(objectToPickUp.gameObject);

                return;
            }

        }
        
        //Place object on Table
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance,
                tableLayer))
        {
            if (handPosition.childCount == 0) return;
            
            handPosition.GetChild(0).GetComponent<Collider>().enabled = true;
            handPosition.GetChild(0).transform.DOMove(hit.point, .5f);
            handPosition.GetChild(0).SetParent(hit.transform);
        }

        //Register
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, registerLayer))
        {
            if (handPosition.childCount == 0) return;
            if (handPosition.GetChild(0).GetComponent<PickUpObject>().IsClean)
            {
                DestroyImmediate(handPosition.GetChild(0).gameObject);
                score += 5;
                playerSettings.playerMoney = score;
                coinSound.SetActive(true);
                StartCoroutine(TimeToWaitSound(1f));
                UpdateHUD();
            }

            
        }
        
    }

        void PickupObject(GameObject objectToPickup)
        {
            //FIX
            objectToPickup.GetComponent<Collider>().enabled = false;
            objectToPickup.transform.SetParent(handPosition);
            objectToPickup.transform.DOLocalMove(Vector3.zero, 0.2f).WaitForCompletion();
            
            isHoldingObject = true;
        }

        #endregion

        #region Focus Modes

        void EnterFocusMode(Transform camera_Move_Position, Transform look_At_Target, Transform basket_Position,
            LaundryMacineRhythm interaction)
        {
            if (interaction.isBusy) return;
            if (transform.GetChild(0).childCount == 0) return;
            playerActions.Disable();
            canMove = false;
            player_Cam_Original_Transform = Camera.main.transform.position;
            Camera.main.transform.DOMove(camera_Move_Position.position, 1f).OnComplete(() => Camera.main.transform.DOLookAt(look_At_Target.transform.position, 0.5f));
            basket.transform.SetParent(basket_Position);
            basket.transform.DOMove(basket_Position.position, 1f);

        }
        
        void EnterFocusMode(Transform camera_Move_Position, Transform look_At_Target)
        {
            playerActions.Disable();
            canMove = false;
            player_Cam_Original_Transform = Camera.main.transform.position;
            Camera.main.transform.DOMove(camera_Move_Position.position, 1f).OnComplete(() => Camera.main.transform.DOLookAt(look_At_Target.transform.position, 0.5f));


        }

        #endregion

        #endregion

        #region HUD

        void UpdateHUD()
        {
            scoreText.text = $"${score}";
            
        }

        #endregion    
    
    private void OnDestroy()
    {
        playerActions.Disable();
    }

    IEnumerator TimeToWaitSound(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        coinSound.SetActive(false);
    }
    
}


