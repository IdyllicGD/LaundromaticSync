using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {
    InputSystem_Actions playerInput;
    Camera playerCam;
    bool isLookingAtObject;

    [SerializeField] LayerMask interactionLayer;
    [SerializeField] float raycastDistance;

    void Update () {
        RaycastingUpdated ();
    }

    void Awake () {
        playerInput = new InputSystem_Actions ();
        playerInput.Enable();
        playerCam = Camera.main;

        playerInput.Player.Interact.performed += x => PlayerInteract ();
    }

    void PlayerInteract () {
        Ray _ray;
        RaycastHit hit;
        

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, Mathf.Infinity, interactionLayer)) {
            Debug.Log ("Interact With Object");
            
            //object that is hit, go out and run Ilootable script code;
        }
    }

    void RaycastingUpdated () {
        RaycastHit hit;
        
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, interactionLayer)) {
            Debug.DrawRay (playerCam.transform.position, playerCam.transform.forward, Color.blue);
            //Debug.Log ("Looking at object");
            
        }
    }
}
