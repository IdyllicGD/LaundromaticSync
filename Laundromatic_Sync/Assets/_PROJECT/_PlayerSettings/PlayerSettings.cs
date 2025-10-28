using System;
using System.Collections.Generic;
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

    private void Start()
    {
        instance = this;
        Time.timeScale = 1f;
        UpdateMoneyText();
        QualitySettings.vSyncCount = 1;

        UpdateFps(fpsSetting[2]);
    }

    public void ReduceMoney(int amount)
    {
        playerMoney -= amount;
        DayNight.instance.moneySpentThisDay += amount;
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        DayNight.instance.moneyMadeThisDay += amount;
        DayNight.instance.moneyMadeTotal += amount;
        UpdateMoneyText();
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
