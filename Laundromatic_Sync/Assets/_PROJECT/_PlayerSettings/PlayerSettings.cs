using System;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public static PlayerSettings instance;
    public PlayerInteraction playerInteraction;
    public float mouseSensitivity = 100f;
    public int playerMoney;
    //Display size
    //Volume
    //Accessibility settings

    private void Start()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    public void ReduceMoney(int amount)
    {
        playerMoney -= amount;
    }
}
