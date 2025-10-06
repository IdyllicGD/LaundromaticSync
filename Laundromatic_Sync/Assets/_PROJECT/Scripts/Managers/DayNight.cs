using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using Sirenix.OdinInspector;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    public static DayNight instance;
    [Title("Time UI Refrences", null, TitleAlignments.Centered)]
    [SerializeField] TextMeshProUGUI minutesText;
    [SerializeField] TextMeshProUGUI hoursText;
    [SerializeField] TextMeshProUGUI dayText;

    [Title("Current Day Time", null, TitleAlignments.Centered)]
    public int minutes;
    public int hours;
    public int days;

    [Title("SkyBox", "Time skybox will change and to what it will change", TitleAlignments.Centered)]
    [SuffixLabel("Minutes, Hours, Days")]
    [SerializeField] Vector3 sunriseTime;
    [SuffixLabel("Minutes, Hours, Days")]
    [SerializeField] Vector3 eveningTime;
    [SuffixLabel("Minutes, Hours, Days")]
    [SerializeField] Vector3 nightTime;
    [SuffixLabel("Minutes, Hours, Days")]
    [SerializeField] Vector3 sunsetTime;
    [Title("Skybox textures", null, TitleAlignments.Left, false, true)]
    [SerializeField] Material sunriseSkybox;
    [SerializeField] Material eveningSkybox;
    [SerializeField] Material nightSkybox;
    [SerializeField] Material sunsetSkybox;
    [Title("Delivery", null, TitleAlignments.Centered)]
    [SerializeField] GameObject deliveryArea;
    public int delivryAreaMaxAmount;
    [Title("End Day Review Vars", null, TitleAlignments.Centered)]
    [SerializeField] GameObject endDayReviewCanvas;
    //Money made
    [SerializeField] TextMeshProUGUI moneyMadeText;
    public int moneyMadeThisDay;
    //Orders Completed
    [SerializeField] TextMeshProUGUI ordersCompletedTodayText;
    public int ordersCompletedToday;
    //Score
    //overall Score
    [Title("Pass out info", null, TitleAlignments.Centered)]
    [SerializeField] GameObject passOutInfo;
    [SerializeField] TextMeshProUGUI passOutInfoText;
    [SerializeField] RawImage blinkImage;
    [SerializeField] TextMeshProUGUI blinkText;
    [SerializeField] float blinkDuration;
    [SerializeField] float blinkWaitTime;
    [SerializeField] PlayerMovement playerMovement;
    float originalPlayerSpeed;
    


    float tempSeconds;

    bool endOfDayPause = false;

    void Start()
    {

        instance = this;
        UpdateTimeData();
        delivryAreaMaxAmount = ReturnAvailableSpaceOnDeliveryTable();
        StartOfNewDay();

        minutesText.text = minutes.ToString("00");
        hoursText.text = hours.ToString("00");
        dayText.text = days.ToString("00");
    }

    void UpdateTimeData()
    {
        minutesText.text = minutes.ToString("00");
        hoursText.text = hours.ToString("00");
        dayText.text = days.ToString("00");
    }

    public void StartOfNewDay()
    {
        endOfDayPause = false;
        RenderSettings.skybox = sunriseSkybox;
        NpcManager.instance.canSpawnNpc = true;
        
    }

    #region End Day Review
    public void EndDayReview()
    {

    }



    #endregion

    void Update()
    {
        if (endOfDayPause) return;
        ChangeSkybox();
        tempSeconds += Time.deltaTime;
        if (tempSeconds >= 1)
        {
            minutes++;
            if (minutes >= 60)
            {
                hours++;
                if (hours == 22)
                {
                    Blink();
                }
                if (hours >= 24)
                {
                    hours = 0;
                    endOfDayPause = true;
                    PauseTimeAndCloseStore();

                }
                hoursText.text = hours.ToString("00");
                minutes = 0;
            }
            minutesText.text = minutes.ToString("00");
            tempSeconds = 0;
        }




    }
    void ChangeSkybox()
    {
        if (hours == eveningTime.y)
        {
            RenderSettings.skybox = eveningSkybox;
        }
        else if (hours == sunsetTime.y)
        {
            RenderSettings.skybox = sunsetSkybox;
        }
        else if (hours == nightTime.y)
        {
            RenderSettings.skybox = nightSkybox;
        }
    }

    void Blink()
    {
        blinkImage.DOFade(1f, blinkDuration).OnComplete(() => StartCoroutine(TimeToWaitForFade(blinkWaitTime)));
        blinkText.DOFade(1f, blinkDuration);
        Camera.main.DOShakePosition(5f, 3f, 10, 90f, true);
        playerMovement.playerSpeed /= 0.5f;

        //Yawn Sound
    }

    IEnumerator TimeToWaitForFade(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        blinkImage.DOFade(0f, blinkDuration);
        blinkText.DOFade(0f, blinkDuration);
        playerMovement.playerSpeed = playerMovement.playerOriginalSpeed;

    }

    public void PauseTimeAndCloseStore()
    {
        NpcManager.instance.canSpawnNpc = false;
    }

    public void EndDay()
    {

        int currentNpcCount = NpcManager.instance.currentNpcs.Count;
        int basketCount = NpcManager.instance.currentBaskets.Count;
        int nullCountBasket = 0;
        int nullCountNpc = 0;
        Debug.Log(currentNpcCount);

        for (int i = 0; i < currentNpcCount; i++)
        {
            if (NpcManager.instance.currentNpcs[i].gameObject == null)
            {
                nullCountNpc++;
            }
            else
            {
                Destroy(NpcManager.instance.currentNpcs[i].gameObject);
            }

        }
        for (int i = 0; i < basketCount; i++)
        {
            if (NpcManager.instance.currentBaskets[i].gameObject == null)
            {
                nullCountBasket++;
            }
            else
            {
                Destroy(NpcManager.instance.currentBaskets[i].gameObject);
            }


        }
        if (nullCountBasket != 0)
        {
            for (int i = 0; i < nullCountBasket; i++)
            {
                NpcManager.instance.currentBaskets.RemoveAt(0);
            }
        }
        if (nullCountNpc != 0)
        {
            for (int i = 0; i < nullCountNpc; i++)
            {
                NpcManager.instance.currentNpcs.RemoveAt(0);
            }
        }


        endOfDayPause = true;
        NpcManager.instance.currentNpcs.Clear();
        NpcManager.instance.currentBaskets.Clear();
        NpcManager.instance.ResetNpcList();
        hours = 6;
        minutes = 0;
        days++;
        PlayerSettings.instance.playerInteraction.StopAndStartPlayerMovement(true);
        UpdateTimeData();
        NpcManager.instance.SpawnNpc();
        RenderSettings.skybox = sunriseSkybox;
        EndDayReview();
    }


    public int ReturnAvailableSpaceOnDeliveryTable()
    {
        int amountAvailable = 0;
        Debug.Log(deliveryArea.transform.childCount);
        for (int i = 0; i <= deliveryArea.transform.childCount - 2; i++)
        {
            if (!CheckIfObjectHasChild(deliveryArea.transform.GetChild(i)))
            {
                amountAvailable++;
                Debug.Log(amountAvailable);
            }
        }
        return amountAvailable;
    }

    bool CheckIfObjectHasChild(Transform objectToCheck)
    {
        if (objectToCheck.childCount != 0)
        {
            return true;
        }
        else return false;
    }

    IEnumerator TimeToWait(float time)
    {
        yield return new WaitForSeconds(time);
        ShopManager.instance.orderList.Clear();

    }

    
}
