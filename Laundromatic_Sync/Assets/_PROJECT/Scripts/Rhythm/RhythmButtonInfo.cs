
using System;
using System.Collections;
using _PROJECT.Scripts.Rhythm;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RhythmButtonInfo : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RhythmButtons rhythmButtons = new RhythmButtons();
    [SerializeField] private float bpm;
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
                    transform.parent.parent.parent.GetComponent<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
                }
                break;
            case RhythmButtons.B:
                if (rhythmButtons == RhythmButtons.B)
                {
                    transform.parent.parent.parent.GetComponent<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
                }
                break;
            case RhythmButtons.X:
                if (rhythmButtons == RhythmButtons.X)
                {
                    transform.parent.parent.parent.GetComponent<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
                }
                break;
            case RhythmButtons.Y:
                if (rhythmButtons == RhythmButtons.Y)
                {
                    transform.parent.parent.parent.GetComponent<RythmStages>().StageCheck();
                    //transform.parent.parent.GetComponent<ButtonSpawner>().SpawnButton();
                    DestroyImmediate(gameObject);
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
}

public enum RhythmButtons{
A, B, X, Y
}