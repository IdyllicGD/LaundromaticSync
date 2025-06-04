using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DryerMachine : MonoBehaviour, I_Interactable_Machine
{
    bool isBusy = false;
    private int stage;

    private MachineFocusMode_Actions machineActions;
    
    #region Sounds
    
    #endregion

    #region Animation Ref

    [SerializeField] private GameObject door;
    
    #endregion

    private float timerTime;
    
    #region rythm Ref
    [SerializeField] private List<Transform> rhythmPositions = new List<Transform>();
    [SerializeField] private List<Sprite> rhythmButtons = new List<Sprite>();
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject rhythmCircle;
    [SerializeField] private float bpm = 30f;
    
    private Sprite firstButtonSprite;
    private Sprite secondButtonSprite;
    private Sprite thirdButtonSprite;
    private Sprite fourthButtonSprite;
    
    #endregion
    
    
    public Vector3 originalCamPosition;

    private void Start()
    {
        machineActions = new MachineFocusMode_Actions();
        machineActions.Rhythm.XAXbox.performed += x => ButtonPressed(0);
        machineActions.Rhythm.SquareXXbox.performed += x => ButtonPressed(1);
        machineActions.Rhythm.TriangleYXbox.performed += x => ButtonPressed(2);
        machineActions.Rhythm.ScicleBXbox.performed += x => ButtonPressed(3);
        
    }

    void ButtonPressed(int buttonPressed)
    {
        switch (buttonPressed)
        {
            case 0:
                Debug.Log("South Button Pressed");
                CheckCurrentButton(0);
                break;
            case 1:
                Debug.Log("East Button Pressed");
                CheckCurrentButton(1);
                break;
            case 2:
                Debug.Log("North Button Pressed");
                CheckCurrentButton(2);
                break;
            case 3:
                Debug.Log("West Button Pressed");
                CheckCurrentButton(3);
                break;
        }
    }

    void CheckCurrentButton(int buttonPressed)
    {
        
    }

    public void Interact()
    {
        stage = 0;
        RhythmPattern();
    }

    void RhythmPattern()
    {
        
    }
}
