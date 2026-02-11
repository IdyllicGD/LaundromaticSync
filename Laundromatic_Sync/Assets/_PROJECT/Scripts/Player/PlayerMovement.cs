using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [Title("Character Controller Vars", null, TitleAlignments.Centered)]
    public float playerSpeed; 
    [SerializeField] float sprintMultiplier;
    [SerializeField] float minClampDistance;
    [SerializeField] float maxClampDistance;
    bool canSprint;
    [Title("Jumping Var", null, TitleAlignments.Centered)]
    [Range(100f, 1000f)]
    public float jumpForce;
    [SerializeField] float maxJumpUpDistance;
    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform feetPosition;
    [Title("Kicking Vars", null, TitleAlignments.Centered)]
    [SerializeField] LayerMask kickLayer;
    [SerializeField] float kickAcelerationAmount;
    [Title("Menu", null, TitleAlignments.Centered)]
    [SerializeField] GameObject pauseCanvas;
    
    [ReadOnly]public float playerOriginalSpeed;
    private PlayerSettings playerSettings;
    [HideInInspector] public InputSystem_Actions inputActions;
    [HideInInspector] public bool canMoveAndLook = true;
    [HideInInspector] public Vector3 cameraOriginalTransform;
    [HideInInspector] public Quaternion cameraOriginalRotation;
    [SerializeField] GameObject croshair;
    [SerializeField] GameObject npcPopUp;
    [Title("Collision Fix Vars")]
    [SerializeField] List<Transform> rayPositions = new();
    [SerializeField] float rayMaxDistance;
    [SerializeField] LayerMask layerMaskForCollision;
    [SerializeField] Transform originalPosition;
    bool isMenuOpen;

    #region Player Settings Variables
    private float mouseSensitivity;
    
    #endregion
    
    #region Character Controller Variables

    public Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 moveDirection;
    private Rigidbody playerRb;
    private float rotateX, rotateY;
    private Camera playerCam;
    private bool canJump = true;
    

    #endregion

    private void FixedUpdate()
    {
        PlayerMove();
        PlayerLook();
    }



    private void Start()
    {
        Menu.setThisGameObjectActive += SetThisGameObjectActive;
        StartCoroutine(TimeToWait(0.1f));
        Cursor.lockState = CursorLockMode.Locked;
        #region Input Actions

        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += x => moveInput = Vector2.zero;

        inputActions.Player.Look.performed += OnLook;

        inputActions.Player.Sprint.performed += x => PlayerSprint();
        inputActions.Player.Sprint.canceled += x => playerSpeed = playerOriginalSpeed;

        inputActions.Player.Kick.performed += x => Kick();

        inputActions.Player.Pause.performed += x => PauseGame();

        inputActions.Player.Jump.performed += x => PlayerJump();
        #endregion

        #region Get Components

        playerRb = GetComponent<Rigidbody>();
        playerSettings = GetComponent<PlayerSettings>();
        playerCam = Camera.main;

        #endregion

        #region Player Settings

        mouseSensitivity = playerSettings.mouseSensitivity;

        #endregion

        #region Other

        playerOriginalSpeed = playerSpeed;

        #endregion
    }

    void OnCollisionEnter(Collision collision)
    {
        canJump = true;
        canSprint = true;
    }
    void SetThisGameObjectActive(GameObject objectToDisableAndEnable)
    {
        objectToDisableAndEnable.SetActive(false);
        objectToDisableAndEnable.SetActive(true);
    }


    public void PauseGame()
    {
        if (isMenuOpen)
        {
            inputActions.Enable();
            croshair.SetActive(true);
            npcPopUp.SetActive(true);
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isMenuOpen = false;
        }
        else
        {
            inputActions.Disable    ();
            Time.timeScale = 0;
            croshair.SetActive(false);
            npcPopUp.SetActive(false);
            pauseCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isMenuOpen = true;
        }


    }

    IEnumerator TimeToWait(float time)
    {
        yield return new WaitForSeconds(time);
        NpcManager.instance.playerTransform = this.transform;
    }


    #region Character Controller

    public void OnMove(InputAction.CallbackContext obj)
    {
        if (!canMoveAndLook) return;
        moveInput = obj.ReadValue<Vector2>();
    }

    public void OnLook (InputAction.CallbackContext obj) {
        if (!canMoveAndLook) return;
        lookInput = obj.ReadValue<Vector2>();
        
    }

    void PlayerMove()
    {
        if (moveInput == Vector2.zero) return;
        float currentMoveX = moveInput.x;
        float currentMoveY = moveInput.y;

        float moveDirectionX = transform.position.x + moveInput.x;
        float moveDirectionY = transform.position.z + moveInput.y;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        moveDirection = camForward * moveInput.y + camRight * moveInput.x;



        playerRb.MovePosition(transform.position + moveDirection * playerSpeed * Time.deltaTime);
        Debug.Log($"x: {currentMoveX} y: {currentMoveY}");


    }


    public void PlayerLook()
    {
        if (!inputActions.Player.Look.inProgress) return;

        float mouseX = lookInput.x * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.fixedDeltaTime;
        rotateX = Mathf.Clamp(rotateX, minClampDistance, maxClampDistance);
        rotateX -= mouseY;

        playerCam.transform.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

    }

    void PlayerJump()
    {
        Debug.Log("Jump Pressed");
        if (!Physics.Raycast(feetPosition.position, -feetPosition.up, out RaycastHit floorTarget, maxJumpUpDistance, floorLayer)) return;
        canJump = false;
        canSprint = false;
        playerRb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Force);
        Debug.Log("Jumped");
    }

    void PlayerSprint()
    {
        if(!Mathf.Approximately(playerSpeed, playerOriginalSpeed) || !canSprint) return;
        playerSpeed *= sprintMultiplier;
    }

    void Kick()
    {
        GameObject objectToKick;
        Ray ray;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit itemHit, kickLayer))
        {
            if (!itemHit.transform.TryGetComponent(out Rigidbody rigidbodyToKick)) return;
            rigidbodyToKick.AddForce(Camera.main.transform.forward * kickAcelerationAmount , ForceMode.Force);
            Debug.Log($"[PlayerMovement] Kick Object"); 
        }
    }



    #endregion

    public void ResetCamMovement()
    {
        lookInput = Vector2.zero;
    }

    public void ResetSpeed()
    {
        playerSpeed = playerOriginalSpeed;
    }

    public void ResetCamera()
    {
        playerCam.transform.DOLocalMove(Vector3.zero, 0.5f);
        playerCam.transform.DORotateQuaternion(cameraOriginalRotation, 0.5f).OnComplete(DoRotateCompletion);
    }

    void DoRotateCompletion()
    {
        inputActions.Enable();
        canMoveAndLook = true;
        PlayerInteraction playerInteraction = transform.GetComponent<PlayerInteraction>();
        playerInteraction.canInteract = true;
    }

    public void ResetMouseSensitivity()
    {
        mouseSensitivity = playerSettings.mouseSensitivity;
    }
    private void OnDestroy()
    {
        inputActions.Disable();
        Menu.setThisGameObjectActive -= SetThisGameObjectActive;
    }
}
