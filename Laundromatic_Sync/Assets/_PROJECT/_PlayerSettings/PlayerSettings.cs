using System;
using TMPro;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings instance;
    public PlayerInteraction playerInteraction;
    public float mouseSensitivity;
    public int playerMoney;
    [SerializeField] TextMeshProUGUI moneyText;
    //Display size
    //Volume
    //Accessibility settings

    private void Start()
    {
        instance = this;
        Time.timeScale = 1f;
        UpdateMoneyText();
    }

    public void ReduceMoney(int amount)
    {
        playerMoney -= amount;
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyText();
    }

    void UpdateMoneyText()
    {
        moneyText.text = playerMoney.ToString("000");
    }
}
