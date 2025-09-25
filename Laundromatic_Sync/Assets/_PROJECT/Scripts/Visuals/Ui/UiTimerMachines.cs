using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UiTimerMachines : MonoBehaviour
{
    [Title("Outside Inward Refrences")]
    public float timerTime;
    public bool hasDetergent, hasFabricSoftener, hasBasket, isDm;

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
        StartCoroutine(TimeToWait(1f));
        

    }

    IEnumerator TimeToWait(float time)
    {

        while (timerTime > 0)
        {
            Debug.Log($"[UiTimerMachines] timerTime = {timerTime}");
            yield return new WaitForSeconds(time);
            timerTime--;
            timeText.text = timerTime.ToString("00");

        }
        if (timerTime <= 0)
        {
            canvasTimer.SetActive(false);
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
        if (!isDm)
        {
            detergent.SetActive(true);
            fabricSoftener.SetActive(true);
            basket.SetActive(true);
        }
        
        hasDetergent = false;
        hasFabricSoftener = false;
        hasBasket = false;
        timerTime = 0;

        canvasGroup.SetActive(true);
        canvasTimer.SetActive(false);
    }
}
