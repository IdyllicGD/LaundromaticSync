using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaundryMacineRhythm : MonoBehaviour, I_Interactable_Machine
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

    [SerializeField] private GameObject openDoorSound;
    [SerializeField] private GameObject  closeDoorSound;
    [SerializeField] private GameObject  coinSound;
    [SerializeField] private GameObject  machineStartSound;
    [SerializeField] private GameObject  machineEndSound;
    [SerializeField] private GameObject  machineBusySound;
    [SerializeField] private GameObject  machineEmtySound;
    [SerializeField] private GameObject  dialSound;

    public Vector3 originalCamPosition;

    private void Start()
    {
        //Initialize the Actions
        machineActions = new MachineFocusMode_Actions();
        machineActions.Rhythm.XAXbox.performed += x => SouthButtonPressed();
        machineActions.Rhythm.SquareXXbox.performed += x => EastButtonPressed();
        machineActions.Rhythm.TriangleYXbox.performed += x => NorthButtonPressed();
        machineActions.Rhythm.ScicleBXbox.performed += x => WestButtonPressed();
        
    }

    public void Interact()
    {
        if (isBusy) return;
        //Make Action map active
        StartCoroutine(TimeToWait(2f));
    }

    void SouthButtonPressed()
    {
        Debug.Log("South Button Pressed");
        //put coin in machine
        
        //play sound
        
        if (stage != 0) return;
        
        
        rhythmPositions[currentRhythmListPosition].transform.GetChild(0).GetComponent<Image>().DOKill();
        
        rhythmPositions[currentRhythmListPosition].GetChild(0).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        currentRhythmListPosition++;
        rhythmPositions[currentRhythmListPosition].GetChild(0).GetComponent<Image>().color = new Color(1f, 1, 1, 1f);
        rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<Image>().color = new Color(1f, 1, 1, 1f);
        stage = 1;
        Debug.Log($"[Play Coin Sound]");
        coinSound.SetActive(true);
        Debug.Log($"[Play Open Door Sound]");
        openDoorSound.SetActive(true);
        //open door
        door.DOLocalRotate(new Vector3(0f, -160f, 0f), 1f, RotateMode.Fast);
        LaundryRhythm(bpm);
    }

    void EastButtonPressed()
    {
        Debug.Log("East Button Pressed");
        if (stage != 1) return;
        rhythmPositions[currentRhythmListPosition].transform.GetChild(0).GetComponent<Image>().DOKill();
        
        rhythmPositions[currentRhythmListPosition].GetChild(0).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        currentRhythmListPosition++;
        rhythmPositions[currentRhythmListPosition].GetChild(0).GetComponent<Image>().color = new Color(1f, 1, 1, 1f);
        rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<Image>().color = new Color(1f, 1, 1, 1f);
        stage = 2;
        Destroy(transform.GetChild(0).GetChild(0).gameObject);
        coinSound.SetActive(false);
        openDoorSound.SetActive(false);
        Debug.Log($"[Play Close Door Sound]");
        closeDoorSound.SetActive(true);
        //close door
        door.DOLocalRotate(new Vector3(0f, 0f, 0f), 1f, RotateMode.Fast);
        LaundryRhythm(bpm);
    }

    void NorthButtonPressed()
    {
        Debug.Log("North Button Pressed");
        if (stage != 2) return;
        rhythmPositions[currentRhythmListPosition].transform.GetChild(0).GetComponent<Image>().DOKill();
        
        rhythmPositions[currentRhythmListPosition].GetChild(0).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        currentRhythmListPosition++;
        rhythmPositions[currentRhythmListPosition].GetChild(0).GetComponent<Image>().color = new Color(1f, 1, 1, 1f);
        rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<Image>().color = new Color(1f, 1, 1, 1f);
        stage = 3;
        closeDoorSound.SetActive(false);
        Debug.Log($"[Play Twist Nob Sound]");
        dialSound.SetActive(true);
        //twist nob
        nob.DOLocalRotate(new Vector3(60f, 0f, 0f), 1f, RotateMode.Fast);
        LaundryRhythm(bpm);
    }

    void WestButtonPressed()
    {
        Debug.Log("West Button Pressed");
        if (stage != 3) return;
        rhythmPositions[currentRhythmListPosition].transform.GetChild(0).GetComponent<Image>().DOKill();
        
        rhythmPositions[currentRhythmListPosition].GetChild(0).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<Image>().color = new Color(1f, 1, 1, 0f);
        currentRhythmListPosition++;
        stage = 0;
        dialSound.SetActive(false);
        Debug.Log($"[Play Lever Sound]");
        machineBusySound.SetActive(true);
        //pull lever
        lever.DOLocalRotate(new Vector3(0f, 0f, 35f), 0.5f, RotateMode.Fast).OnComplete(() => lever.DOLocalRotate(new Vector3(0f,0f,-47.908f), 0.5f, RotateMode.Fast));
        MachineStart();
        LaundryRhythm(bpm);
        
    }
    
    void LaundryRhythm(float bpm)
    {
        if (currentRhythmListPosition >= rhythmPositions.Count)
        {
            currentRhythmListPosition = 0;
            stage = 0;
            //Exit Focus Mode
            ExitFocusMode();
            //Start Machine
            return;
        }
        
        Image image = rhythmPositions[currentRhythmListPosition].transform.GetChild(0).GetComponent<Image>();
        RectTransform rectTransformOfCircle = rhythmPositions[currentRhythmListPosition].GetChild(1).GetComponent<RectTransform>();
        float timeInSeconds = bpm / 60f;
        
        InteractionAnimation(image, rectTransformOfCircle, timeInSeconds);
        
    }

    void InteractionAnimation(Image image, RectTransform rectTransformOfCircle, float timeInSeconds)
    {
        image.DOFade(1, timeInSeconds).SetLoops(-1, LoopType.Yoyo);
        rectTransformOfCircle.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        rectTransformOfCircle.DOScale(1, timeInSeconds).SetLoops(-1, LoopType.Yoyo);
    }
    

    void MachineStart()
    {
        StartCoroutine(TimeToWaitAtEnd(5f));
        isBusy = true;
    }

    void ExitFocusMode()
    {
        Camera.main.GetComponentInParent<Character_Controller_Player>().playerActions.Enable();
        Camera.main.transform.DOMove(originalCamPosition, 0.5f).OnComplete(() => Camera.main.GetComponentInParent<Character_Controller_Player>().canMove = true);
        machineActions.Disable();
    }

    IEnumerator TimeToWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rhythmPositions[currentRhythmListPosition].gameObject.SetActive(true);
        machineActions.Enable();
        LaundryRhythm(bpm);
        currentRhythmListPosition = 0;
        stage = 0;
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
