using UnityEngine;
using UnityEngine.InputSystem;

public class Character_Controller_Player : MonoBehaviour
{
    InputSystem_Actions playerActions;
    Rigidbody playerRb;
    Vector3 moveDirection;
    Vector2 moveInput, lookInput;
    float rotateX, rotateY;
    Camera playerCam;
    
    //Serialize fields
    [SerializeField] float playerSpeed, 
                           minClampDistance, 
                           maxClampDistance, 
                           mouseSensitivity;
    
    

    void FixedUpdate () {
        PlayerMovement ();
    }

    void Awake () {
        Cursor.lockState = CursorLockMode.Locked;
        playerCam = Camera.main;
        playerRb = GetComponent<Rigidbody>();
        playerActions = new InputSystem_Actions ();
        playerActions.Enable();
        
        //playerInputs
        playerActions.Player.Move.performed += OnMove;
        playerActions.Player.Move.canceled += OnMove;
        playerActions.Player.Look.performed += OnLook;
        playerActions.Player.Look.canceled += OnLook;
        playerActions.Player.Look.performed += x => MouseLookAround();
    }

    public void OnMove (InputAction.CallbackContext obj) {
        moveInput = obj.ReadValue<Vector2> ();
        
    }

    public void OnLook (InputAction.CallbackContext obj) {
        lookInput = obj.ReadValue<Vector2> ();
    }

    void PlayerMovement () {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        moveDirection = camForward * moveInput.y + camRight * moveInput.x;
        playerRb.linearVelocity =  moveDirection * (playerSpeed * Time.fixedDeltaTime);

    }
    
    public void MouseLookAround () {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        
        rotateX = Mathf.Clamp(rotateX, minClampDistance, maxClampDistance);
        rotateX -= mouseY;
        
        playerCam.transform.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
        transform.Rotate (Vector3.up * mouseX);
        
    }

    
    
}


