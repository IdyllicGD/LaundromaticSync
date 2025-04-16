using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    bool isLookingAtObject;
    bool canMove = true;
    
    //Serialize fields
    [SerializeField] float playerSpeed, minClampDistance, maxClampDistance, mouseSensitivity;
    [SerializeField] Vector3 player_Cam_Original_Transform;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] float raycastDistance;

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
        playerActions.Player.Interact.performed += x => PlayerInteract ();
        playerActions.Player.Exit_Focus_Mode.performed += x => ExitFocusMode();
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

    #region Interaction

    void PlayerInteract () {
        if(!canMove) return;
        RaycastHit hit;
        
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, interactionLayer)) {
            //Debug.Log ($"[Interact With Interactable Object] [{hit.collider.gameObject.name}]");
            Machine_Interaction _interactable_Machine = hit.collider.gameObject.GetComponent<Machine_Interaction> ();
            if (_interactable_Machine != null) { }
            EnterFocusMode(_interactable_Machine.machine_Cam_Transform, _interactable_Machine.look_At_Target);
            _interactable_Machine.Interact();
            //object that is hit, go out and run I_Interactable_Machine Interface type script code;
        }
    }

    void EnterFocusMode(Transform camera_Move_Position, Transform look_At_Target)
    {
        canMove = false;
        player_Cam_Original_Transform = Camera.main.transform.position;
        Camera.main.transform.DOMove(camera_Move_Position.position, 1f);
        
        StartCoroutine(TimeToComplete(1.1f));
        
        IEnumerator TimeToComplete(float time)
        {
            yield return new WaitForSeconds(time);
            float currentTime = 0;
        
            while (currentTime < time)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= 1.0f)
                {
                    StartMove();
                    break;
                }
                
            }
           
        }
        
        void StartMove()
        {
            Camera.main.transform.DOLookAt(look_At_Target.transform.position, 0.5f);
        }
        
    }

    void ExitFocusMode()
    {
        if (canMove) return;
        Camera.main.transform.DOMove(player_Cam_Original_Transform, 0.5f);
        canMove = true;
    }

    // void RaycastingUpdated () {
    //     RaycastHit hit;
    //     Debug.DrawRay (playerCam.transform.position, playerCam.transform.forward, Color.blue);
    //     if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, interactionLayer)) {
    //         Debug.DrawRay (playerCam.transform.position, playerCam.transform.forward, Color.blue);
    //         //Debug.Log ("Looking at object");
    //         
    //     }
    // }

    #endregion
    

    private void OnDestroy()
    {
        playerActions.Disable();
    }
    
}


