using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings instance;
    public PlayerInteraction playerInteraction;
    public float mouseSensitivity;
    public int playerMoney;
    [SerializeField] TextMeshProUGUI moneyText;
    public List<int> fpsSetting = new();

    [Title("Animation", null, TitleAlignments.Centered)]
    [SerializeField] float animHeightWin = 160f;
    [SerializeField] float animHeightLose;
    [SerializeField] float animSpeed;
    [SerializeField] float animShake;
    [SerializeField] TextMeshProUGUI moneyGainText;
    [SerializeField] TextMeshProUGUI moneyLoseText;
    [SerializeField] GameObject moneyGain;
    [SerializeField] GameObject moneyLose;
    Vector3 moneyTransformPositionBase;

    bool busyWithAnim;
    bool busyWithLoseAnim;
    bool moneyBeingShook;
    private void Start()
    {
        moneyTransformPositionBase = moneyText.transform.position;
        instance = this;
        Time.timeScale = 1f;
        UpdateMoneyText();
        QualitySettings.vSyncCount = 1;

        UpdateFps(fpsSetting[2]);
    }

    public void ReduceMoney(int amount)
    {
        MoneyShake();
        if (busyWithLoseAnim)
        {
            moneyLose.transform.DOKill();
            moneyLoseText.DOKill();
            ResetLoseText();
        }


        busyWithLoseAnim = true;
        moneyLoseText.text = $"-${amount.ToString("00")}";
        moneyLose.transform.DOLocalMoveY(animHeightLose, animSpeed);
        moneyLoseText.DOFade(0f, animSpeed).OnComplete(() => ResetLoseText());
        playerMoney -= amount;
        DayNight.instance.moneySpentThisDay += amount;
        UpdateMoneyText();
    }

    void ResetLoseText()
    {
        moneyLose.transform.DOLocalMoveY(-120.7f, 0f);
        moneyLoseText.text = string.Empty;
        moneyLoseText.DOFade(1f, 0);
        busyWithLoseAnim = false;

    }

    [Button]
    public void AddSomeMoola()
    {
        AddMoney(10);
    }

    [Button]
    public void RemoveSomeMoola()
    {
        ReduceMoney(10);
    }

    void MoneyShake()
    {
        if (moneyBeingShook)
        {
            moneyText.transform.DOKill();
            ResetMoneyPosition();
        }
        moneyBeingShook = true;
        moneyText.transform.DOShakePosition(animSpeed, animShake).OnComplete(() => ResetMoneyPosition());
    }

    void ResetMoneyPosition()
    {
        moneyText.rectTransform.anchoredPosition = new Vector3(45f, 4.5f, 0f);
        moneyBeingShook = false;
    }

    public void AddMoney(int amount)
    {
        MoneyShake();
        if (busyWithAnim) 
        {
            moneyGain.transform.DOKill();
            moneyGainText.DOKill();
            ResetText();
        }
        busyWithAnim = true;
        moneyGainText.text = $"+${amount.ToString("00")}";
        moneyGain.transform.DOLocalMoveY(animHeightWin, animSpeed);
        moneyGainText.DOFade(0f, animSpeed).OnComplete(() => ResetText());
        playerMoney += amount;
        DayNight.instance.moneyMadeThisDay += amount;
        DayNight.instance.moneyMadeTotal += amount;
        UpdateMoneyText();
    }

    void ResetText()
    {
        moneyGain.transform.DOLocalMoveY(0f, 0f);
        moneyGainText.text = string.Empty;
        moneyGainText.DOFade(1f, 0);
        busyWithAnim = false;

    }

    void UpdateMoneyText()
    {
        moneyText.text = playerMoney.ToString("000");
    }

    public void UpdateFps(int updatedAmount)
    {
        Application.targetFrameRate = updatedAmount;
    }
}
