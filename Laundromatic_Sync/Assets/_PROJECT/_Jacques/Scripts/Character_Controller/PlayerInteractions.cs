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
        
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, interactionLayer)) {
            //Debug.Log ($"[Interact With Interactable Object] [{hit.collider.gameObject.name}]");
            Machine_Interaction _interactable_Machine = hit.collider.gameObject.GetComponent<Machine_Interaction> ();
            _interactable_Machine.Interact();
            //object that is hit, go out and run I_Interactable_Machine Interface type script code;
        }
    }

    void RaycastingUpdated () {
        RaycastHit hit;
        Debug.DrawRay (playerCam.transform.position, playerCam.transform.forward, Color.blue);
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, raycastDistance, interactionLayer)) {
            Debug.DrawRay (playerCam.transform.position, playerCam.transform.forward, Color.blue);
            //Debug.Log ("Looking at object");
            
        }
    }

    private void OnDestroy()
    {
        playerInput.Disable();
    }
}
