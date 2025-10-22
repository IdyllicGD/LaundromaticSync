
using System;
using System.Collections;
using _PROJECT.Scripts.Rhythm;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class RhythmButtonInfo : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RhythmButtons rhythmButtons = new RhythmButtons();
    [SerializeField] private float bpm;
    [SerializeField] bool canBeImpacted;
    [SerializeField] StudioEventEmitter errorSound;
    [SerializeField] float mistakeTimeAdd;
    MachineFocusMode_Actions rhythmActions;

    private void Awake()
    {

        rhythmActions = new MachineFocusMode_Actions();
        rhythmActions.Enable();
        rhythmActions.Rhythm.XAXbox.performed += x => ButtonPressed(RhythmButtons.A);
        rhythmActions.Rhythm.ScicleBXbox.performed += x => ButtonPressed(RhythmButtons.B);
        rhythmActions.Rhythm.TriangleYXbox.performed += x => ButtonPressed(RhythmButtons.Y);
        rhythmActions.Rhythm.SquareXXbox.performed += x => ButtonPressed(RhythmButtons.X);
        StartButtonAnimation();
    }

    void ButtonPressed(RhythmButtons buttonPressed)
    {
        switch (buttonPressed)
        {
            case RhythmButtons.A:
                if (rhythmButtons == RhythmButtons.A)
                {
                    transform.parent.parent.parent.GetComponentInChildren<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
                }
                else
                {
                    errorSound.Play();
                    GetParentScript();
                    if (!canBeImpacted) return;

                    //Minus Points
                }
                break;
            case RhythmButtons.B:
                if (rhythmButtons == RhythmButtons.B)
                {
                    transform.parent.parent.parent.GetComponentInChildren<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
                }
                else
                {
                    GetParentScript();
                    errorSound.Play();
                }
                break;
            case RhythmButtons.X:
                if (rhythmButtons == RhythmButtons.X)
                {
                    transform.parent.parent.parent.GetComponentInChildren<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
                }
                else
                {
                    GetParentScript();
                    errorSound.Play();
                }
                break;
            case RhythmButtons.Y:
                if (rhythmButtons == RhythmButtons.Y)
                {
                    transform.parent.parent.parent.GetComponentInChildren<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
                }
                else
                {
                    GetParentScript();
                    errorSound.Play();
                }
                break;
        }
    }

    void StartButtonAnimation()
    {
        float timeInSeconds = bpm / 60;
        buttonImage.DOFade(1f, timeInSeconds).SetLoops(-1, LoopType.Yoyo);
        rectTransform.DOScale(1f, timeInSeconds).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        rhythmActions.Disable();
    }

    IEnumerator TimeToWait(float time, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(time);
        Destroy(objectToDestroy);
    }

    void GetParentScript()
    {
        // Debug.Log($"[RythmButtonInfo] GetParentScript ran");
        // Debug.Log($"[RythmButtonInfo] {transform.parent.parent.parent.GetChild(1).name} Lm Name");
        // Debug.Log($"[RythmButtonInfo] {transform.parent.parent.parent.name} Dm name");
        //Laundry Machine
        // if (transform.parent.parent.parent.GetChild(1).TryGetComponent(out TimerMachines currentMachine))
        // {
        //     currentMachine.AddTimeToTimer(mistakeTimeAdd);
        // }//Dryer Machine

        // else
        // {
        //     Debug.Log($"[RythButtonInfo] No Mistake Timer");
        // }
    }
}

public enum RhythmButtons{
A, B, X, Y
}