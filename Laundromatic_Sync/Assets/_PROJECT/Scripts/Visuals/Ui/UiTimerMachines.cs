using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UiTimerMachines : MonoBehaviour
{
    [Title("Outside Inward Refrences")]
    public float timerTime;
    public bool hasDetergent, hasFabricSoftener, hasBasket;

    [Title("Local Refrences")]
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject detergent, fabricSoftener, basket;
    [SerializeField] GameObject canvasTimer, canvasGroup;



    public void StartTimerTime(float time)
    {
        timerTime = time;
        timeText.text = timerTime.ToString("00");
        canvasGroup.SetActive(false);
        canvasTimer.SetActive(true);
        
        while (timerTime > 0)
        {
            timeText.text = timerTime.ToString("00");
            timerTime -= Time.deltaTime;
        }
    }

    public void UpdateImages()
    {
        if (hasDetergent)
        {
            detergent.SetActive(false);
        }
        if (hasFabricSoftener)
        {
            fabricSoftener.SetActive(false);
        }
        if (hasBasket)
        {
            basket.SetActive(false);
        }
    }
    public void ResetUiTimer()
    {
        hasDetergent = false;
        hasFabricSoftener = false;
        hasBasket = false;
        timerTime = 0;
        detergent.SetActive(true);
        fabricSoftener.SetActive(true);
        basket.SetActive(true);
        canvasGroup.SetActive(true);
        canvasTimer.SetActive(false);
    }
}
