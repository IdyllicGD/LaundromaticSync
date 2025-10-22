using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTimerMachines : MonoBehaviour
{
    [Title("Outside Inward Refrences", null, TitleAlignments.Centered)]
    public float timerTime;
    public bool hasDetergent, hasFabricSoftener, hasBasket, isDm;

    [Title("Local Refrences", null, TitleAlignments.Centered)]
    public TextMeshProUGUI timeText;
    [SerializeField] Image detergentColour, fabricSoftenerColour, basketColour;
    [Title("Colours on Machine", null, TitleAlignments.Centered)]
    [SerializeField] Color hasObjectColour;
    [SerializeField] Color doesntHaveObjectColour;
    [Title("Canvasas")]
    public GameObject canvasTimer, canvasGroup;
    public bool interupt;



    public void StartTimerTime(float time)
    {

        timerTime = time;
        timeText.text = timerTime.ToString("00");
        canvasGroup.SetActive(false);
        canvasTimer.SetActive(true);
        StartCoroutine(TimeToWait(timerTime));
        

    }

    IEnumerator TimeToWait(float time)
    {

        while (time > 0 && !interupt)
        {
            Debug.Log($"[UiTimerMachines] timerTime = {timerTime}");
            time -= Time.deltaTime;
            timeText.text = time.ToString("00");
            yield return null;

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
            detergentColour.color = hasObjectColour;
        }
        if (hasFabricSoftener)
        {
            fabricSoftenerColour.color = hasObjectColour;
        }
        if (hasBasket)
        {
            basketColour.color = hasObjectColour;
        }
    }
    public void ResetUiTimer()
    {
        interupt = true;
        if (!isDm)
        {
            detergentColour.color = doesntHaveObjectColour;
            fabricSoftenerColour.color = doesntHaveObjectColour;
            basketColour.color = doesntHaveObjectColour;
        }
        
        hasDetergent = false;
        hasFabricSoftener = false;
        hasBasket = false;
        timerTime = 0;

        canvasGroup.SetActive(true);
        canvasTimer.SetActive(false);
    }
}
