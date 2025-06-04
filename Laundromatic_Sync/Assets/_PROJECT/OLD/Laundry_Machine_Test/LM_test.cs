using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class LM_test : MonoBehaviour
{
    [SerializeField] private GameObject doorClosed, doorOpened;
    public bool doorClosedState = true;
    public TMP_Dropdown dropdownLoads;
    public Slider startSlider;
    public int loadScore;
    


    public void ChangeDoorState()
    {
        if (doorClosedState)
        {
            doorClosed.SetActive(false);
            doorOpened.SetActive(true);
            doorClosedState = false;
        }
        else
        {
            doorClosed.SetActive(true);
            doorOpened.SetActive(false);
            doorClosedState = true;
        }
    }

    public void StartMachine()
    {
        if (!doorClosedState) return;
        if (!Mathf.Approximately(startSlider.value, 1)) return;
        startSlider.value = 0;
        switch (dropdownLoads.value)
        {
            case 0:
                loadScore = 25;
                break;
            case 1:
                loadScore = 50;
                break;
            case 2:
                loadScore = 100;
                break;
        }
    }

    
}
