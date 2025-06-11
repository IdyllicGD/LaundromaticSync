using System;
using System.Collections;
using System.Collections.Generic;
using _PROJECT.Scripts.Rhythm;
using DG.Tweening;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaundryMacineRhythm : MonoBehaviour, I_Interactable_Machine, RythmStages
{
    [SerializeField] 
    private float bpm = 60f;

    [SerializeField]
    private GameObject rhythmCircle;
    
    [SerializeField]
    private List<Transform> rhythmPositions = new List<Transform>();
    
    [SerializeField]
    private Canvas canvas;

    private MachineFocusMode_Actions machineActions;

    public Transform camPosition;
    public Transform basketPosition;
    public Transform lookAtPosition;

    public bool isBusy = false;
    public bool hasBasket = false;
    
    private float timerTime;
    
    int currentRhythmListPosition;
    private int stage;

    [SerializeField] private Transform door;
    [SerializeField] private Transform nob;
    [SerializeField] private Transform coin;
    [SerializeField] private Transform lever;
    [SerializeField] private GameObject cleanBasket;
    [SerializeField] private ButtonSpawner buttonSpawner;

    #region Sounds
    [SerializeField] private GameObject openDoorSound;
    [SerializeField] private GameObject  closeDoorSound;
    [SerializeField] private GameObject  coinSound;
    [SerializeField] private GameObject  machineStartSound;
    [SerializeField] private GameObject  machineEndSound;
    [SerializeField] private GameObject  machineBusySound;
    [SerializeField] private GameObject  machineEmtySound;
    [SerializeField] private GameObject  dialSound;
    #endregion

    public Vector3 originalCamPosition;

    private void Start()
    {
        //Initialize the Actions
        // machineActions = new MachineFocusMode_Actions();
        // machineActions.Rhythm.XAXbox.performed += x => SouthButtonPressed();
        // machineActions.Rhythm.SquareXXbox.performed += x => EastButtonPressed();
        // machineActions.Rhythm.TriangleYXbox.performed += x => NorthButtonPressed();
        // machineActions.Rhythm.ScicleBXbox.performed += x => WestButtonPressed();
        
    }

    public void Interact()
    {
        if (isBusy) return;
        //Make Action map active
        StartCoroutine(TimeToWait(2f));
        
    }

    public void StageCheck()
    {
        switch (stage)
        {
            case 0:
                FirstInteract();
                break;
            case 1:
                SecondInteract();
                break;
            case 2:
                ThirdInteract();
                break;
            case 3:
                FourthInteract();
                break;
            
        }
    }

    public void FirstInteract()
    {
        Debug.Log($"[Play Coin Sound]");
        coinSound.SetActive(true);
        Debug.Log($"[Play Open Door Sound]");
        openDoorSound.SetActive(true);
        //open door
        door.DOLocalRotate(new Vector3(0f, -160f, 0f), 1f, RotateMode.Fast);
        
        stage = 1;
        buttonSpawner.SpawnButton();
    }

    public void SecondInteract()
    {
        
        Destroy(transform.GetChild(0).GetChild(0).gameObject);
        coinSound.SetActive(false);
        openDoorSound.SetActive(false);
        Debug.Log($"[Play Close Door Sound]");
        closeDoorSound.SetActive(true);
        //close door
        door.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f, RotateMode.Fast);
        stage = 2;
        buttonSpawner.SpawnButton();
    }

    public void ThirdInteract()
    {
        
        closeDoorSound.SetActive(false);
        Debug.Log($"[Play Twist Nob Sound]");
        dialSound.SetActive(true);
        //twist nob
        nob.DOLocalRotate(new Vector3(60f, 0f, 0f), 1f, RotateMode.Fast);
        stage = 3;
        buttonSpawner.SpawnButton();
    }

    public void FourthInteract()
    {
        
        
        dialSound.SetActive(false);
        Debug.Log($"[Play Lever Sound]");
        machineBusySound.SetActive(true);
        //pull lever
        lever.DOLocalRotate(new Vector3(0f, 0f, 35f), 0.5f, RotateMode.Fast).OnComplete((OnCompleteDoThis));
        stage = 0;
    }

    void OnCompleteDoThis()
    {
        lever.DOLocalRotate(new Vector3(0f,0f,-47.908f), 0.5f, RotateMode.Fast).OnComplete(ExitFocusMode);
        MachineStart();
    }
    

    void MachineStart()
    {
        StartCoroutine(TimeToWaitAtEnd(15f));
        buttonSpawner.currentStage = 0;
        isBusy = true;
    }

    void ExitFocusMode()
    {
        Camera.main.transform.DOMove(originalCamPosition, 0.5f).OnComplete(() => Camera.main.GetComponentInParent<Character_Controller_Player>().canMove = true);
        //machineActions.Disable();
        Camera.main.GetComponentInParent<Character_Controller_Player>().playerActions.Enable();
        stage = 0;
    }

    IEnumerator TimeToWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        currentRhythmListPosition = 0;
        stage = 0;
        buttonSpawner.SpawnButton();
    }

    IEnumerator TimeToWaitAtEnd(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isBusy = false;
        Instantiate(cleanBasket, transform.GetChild(0));
        hasBasket = true;
        
        machineBusySound.SetActive(false);
        if (machineEndSound.activeInHierarchy)
        {
            machineEndSound.SetActive(false);
        }
        machineEndSound.SetActive(true);
        
    }

}
