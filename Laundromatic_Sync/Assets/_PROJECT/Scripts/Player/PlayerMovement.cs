using System;
using System.Collections;
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
    [SerializeField] float playerSpeed; 
    [SerializeField] float sprintMultiplier;
    [SerializeField] float minClampDistance;
    [SerializeField] float maxClampDistance;
    [Title("Kicking Vars", null, TitleAlignments.Centered)]
    [SerializeField] LayerMask kickLayer;
    [SerializeField] float kickAcelerationAmount;
    [Title("Menu", null, TitleAlignments.Centered)]
    [SerializeField] GameObject pauseCanvas;
    
    private float playerOriginalSpeed;
    private PlayerSettings playerSettings;
    [HideInInspector] public InputSystem_Actions inputActions;
    [HideInInspector] public bool canMoveAndLook = true;
    [HideInInspector] public Vector3 cameraOriginalTransform;
    [HideInInspector] public Quaternion cameraOriginalRotation;

    #region Player Settings Variables
    private float mouseSensitivity;
    
    #endregion
    
    #region Character Controller Variables

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 moveDirection;
    private Rigidbody playerRb;
    private float rotateX, rotateY;
    private Camera playerCam;

    #endregion


    private void LateUpdate()
    {
        PlayerMove();
        PlayerLook();
    }

    private void Start()
    {
        
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

    void PauseGame()
    {
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

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
        lookInput = obj.ReadValue<Vector2> ();
    }

    void PlayerMove()
    {      
        if (moveInput == Vector2.zero) return;

        transform.Translate(moveInput.x * Time.deltaTime * playerSpeed, 0, moveInput.y * Time.deltaTime * playerSpeed);

    }
    
    public void PlayerLook () {
        if (!inputActions.Player.Look.inProgress)return;
        
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        rotateX = Mathf.Clamp(rotateX, minClampDistance, maxClampDistance);
        rotateX -= mouseY;
        
        playerCam.transform.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
        transform.Rotate (Vector3.up * mouseX);
        
    }

    void PlayerSprint()
    {
        if(!Mathf.Approximately(playerSpeed, playerOriginalSpeed)) return;
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
            //play kick sound



            //play kick sound
            //kick object   
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
    }
}
