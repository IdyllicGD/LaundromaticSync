
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RhythmButtonInfo : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RhythmButtons rhythmButtons = new RhythmButtons();
    [SerializeField] private float bpm;

    private void Awake()
    {
        StartButtonAnimation();
    }

    void StartButtonAnimation()
    {
        float timeInSeconds = bpm / 60;
        buttonImage.DOFade(1f, timeInSeconds).SetLoops(-1, LoopType.Yoyo);
        rectTransform.DOScale(1f, timeInSeconds).SetLoops(-1, LoopType.Yoyo);
    }
}

public enum RhythmButtons{
A, B, X, Y
}