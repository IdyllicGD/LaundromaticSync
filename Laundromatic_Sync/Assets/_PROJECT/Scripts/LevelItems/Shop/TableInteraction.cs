using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TableInteraction : MonoBehaviour
{
    [SerializeField] public Transform camPos;
    [SerializeField] public Transform camLookAtPos;
    [SerializeField] public Transform originalCamPos;
    [SerializeField] public GameObject mainCanvas;

    private Camera cam;
    
    public InputSystem_Actions _inputAction;

    private void Start()
    {
        cam = Camera.main;
        _inputAction = new InputSystem_Actions();

        _inputAction.Player.Exit_Focus_Mode.performed += x => ExitScreen();
    }
    

    void ExitScreen()
    {
        Camera cam = Camera.main;
        cam.transform.DOMove(originalCamPos.position, 1f).OnComplete(OnCompleteActions);
        mainCanvas.SetActive(false);
    }

    void OnCompleteActions()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _inputAction.Disable();
        Character_Controller_Player.instance.playerActions.Enable();
    }
}
