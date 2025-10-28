using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using FMODUnity;
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
    [Title("Start of Day Pages", null, TitleAlignments.Centered)]
    //[SerializeField] GameObject startOfDayCanvas;
    [SerializeField] GameObject pageOne;
    [SerializeField] GameObject pageTwo;
    [SerializeField] GameObject finalPage;
    [SerializeField] int amountNeededToWin;

    [Title("End Day Review Vars", null, TitleAlignments.Centered)]
    [SerializeField] TextMeshProUGUI moneyMadeTextTotal;
    public int moneyMadeTotal;
    [SerializeField] TextMeshProUGUI moneyNeedToMakeText;
    public int moneyNeedToMakeTotal;
    //Money made
    [SerializeField] TextMeshProUGUI moneyMadeText;
    public int moneyMadeThisDay;
    [SerializeField] TextMeshProUGUI moneySpentText;
    public int moneySpentThisDay;
    //Orders Completed
    [SerializeField] TextMeshProUGUI ordersCompletedTodayText;
    public int ordersCompletedToday;
    [SerializeField] TextMeshProUGUI ordersWrongTodayText;
    public int ordersWrongToday;
    [SerializeField] TextMeshProUGUI todaysScoreText;
    public float todaysStoreScore;
    [SerializeField] MachineManager machineManager;

    [Title("Store Score Info", null, TitleAlignments.Centered)]
    //[SerializeField] GameObject starOne, starTwo, starThree;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float storeScore;
    [SerializeField] float storewrongOrdersMultiplier;
    [SerializeField] float storeOrdersCompleteMultiplier;
    [SerializeField] float storeMoneyMadeTodayMultiplier;

    //Score
    //overall Score
    [Title("Pass out info", null, TitleAlignments.Centered)]
    [SerializeField] RawImage blinkImage;
    [SerializeField] TextMeshProUGUI blinkText;
    [SerializeField] String passOutText;
    [SerializeField] float blinkDuration;
    [SerializeField] float blinkWaitTime;
    [SerializeField] Transform spawnPosition;
    [Title("Camera Shake", null, TitleAlignments.Centered)]
    [SerializeField] float cameraShakeDuration;
    [SerializeField] float cameraShakeStrength;
    [SerializeField] int cameraShakeVibrato;
    [SerializeField] float cameraShakeRandomness;
    [SerializeField] bool cameraShakeFadeout;
    [Title("Other", null, TitleAlignments.Centered)]
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] PlayerMovement playerMovement;
    [Title("End of Day", null, TitleAlignments.Centered)]
    [SerializeField] GameObject winPage;
    [SerializeField] GameObject losePage;
    [SerializeField] TextMeshProUGUI moneyStillNeededText;
    [SerializeField] TextMeshProUGUI moneyStillNeededFinalText;
    [SerializeField] TextMeshProUGUI dayTextOnPaper;
    [SerializeField] TextMeshProUGUI dayTextOnFinalPaper;
    [Title("Win State")]
    [SerializeField] TextMeshProUGUI totalMoneyMadeWinText;
    [Title("Lose State")]
    [SerializeField] TextMeshProUGUI moneyNeededToWinLoseStateText;
    [Title("Sounds")]
    [SerializeField] StudioEventEmitter winSound;
    [SerializeField] StudioEventEmitter loseSound;


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
        StartUpPage();
    }

    #region End Day Review
    public void EndDayReview()
    {
        machineManager.ResetAllMachines();
        todaysStoreScore = 0f;
        blinkImage.DOFade(1f, blinkDuration).OnComplete(() => EndReview());


        float todaysMoneyScore = moneyMadeThisDay * storeMoneyMadeTodayMultiplier;
        float todaysOrdersCompleteScore = ordersCompletedToday * storeOrdersCompleteMultiplier;
        float todaysOrdersWrongScore = ordersWrongToday * storewrongOrdersMultiplier;

        todaysStoreScore += todaysMoneyScore;
        todaysStoreScore += todaysOrdersCompleteScore;
        todaysStoreScore -= todaysOrdersWrongScore;

        storeScore += todaysStoreScore;








    }

    public void EndReview()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInteraction.StopAndStartPlayerMovement(true);
        blinkImage.DOFade(0f, blinkDuration);
        EndDay();
    }

    void StartUpPage()
    {
        Cursor.lockState = CursorLockMode.None;
        playerInteraction.StopAndStartPlayerMovement(false);

        if(moneyMadeTotal > amountNeededToWin)
        {
            WinState();
            return;
        }

        //Start up with a page explaining what the goal is for the player
        switch (days)
        {
            case 1:
                dayTextOnPaper.text = "3 Days Left";
                moneyStillNeededText.text = $"${(amountNeededToWin - moneyMadeTotal).ToString()}";
                pageOne.SetActive(true);
                
                break;
            case 2:
                dayTextOnPaper.text = "2 Days Left";
                moneyStillNeededText.text = $"${(amountNeededToWin - moneyMadeTotal).ToString()}";
                pageOne.SetActive(true);
                break;
            case 3:
                dayTextOnFinalPaper.text = "Final Day Left";
                moneyStillNeededFinalText.text = $"${(amountNeededToWin - moneyMadeTotal).ToString()}"; 
                finalPage.SetActive(true);
                break;
            case 4:
                LoseState();
                break;

        }
    }

    void WinState()
    {
        //Show win screen and let player keep playing
        totalMoneyMadeWinText.text = $"${moneyMadeTotal.ToString()}";
        winPage.SetActive(true);
        winSound.Play();
        //On the page make the player continue with a button and let the player continue or go to main menu
        //Play win sound
    }
    void LoseState()
    {
        //Show lose screen
        moneyNeededToWinLoseStateText.text = "${$(amountNeededToWin - moneyMadeTotal).ToString()}";
        losePage.SetActive(true);
        loseSound.Play();
        //Make the player go to main screen with a button
    }


    public void StartUpDayAgain()
    {
        Cursor.lockState = CursorLockMode.Locked;
        endOfDayPause = false;
        RenderSettings.skybox = sunriseSkybox;
        NpcManager.instance.canSpawnNpc = true;
        tempSeconds = 0f;
        playerInteraction.StopAndStartPlayerMovement(true);
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
                    Blink("GO TO SLEEP");
                }
                if (hours >= 24)
                {
                    hours = 0;
                    endOfDayPause = true;
                    PauseTimeAndCloseStore();
                    PassOut();
                }
                hoursText.text = hours.ToString("00");
                minutes = 0;
            }
            minutesText.text = minutes.ToString("00");
            tempSeconds = 0;
        }




    }

    void PassOut()
    {
        blinkText.text = passOutText;
        blinkText.DOFade(1f, blinkDuration);
        blinkImage.DOFade(1f, blinkDuration).OnComplete(() => Camera.main.transform.parent.parent.transform.DOMove(spawnPosition.position, 0f));
        StartCoroutine(WaitForSecondsThenContinue(3f));
    }

    IEnumerator WaitForSecondsThenContinue(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        blinkText.DOFade(1f, blinkDuration).OnComplete(() => blinkText.text = "");
        blinkImage.DOFade(1f, blinkDuration).OnComplete(() => EndDayReview());
        

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

    void Blink(string textToSet)
    {
        blinkText.text = textToSet;
        blinkImage.DOFade(1f, blinkDuration).OnComplete(() => StartCoroutine(TimeToWaitForFade(blinkWaitTime)));
        blinkText.DOFade(1f, blinkDuration);
        Camera.main.DOShakePosition(cameraShakeDuration, cameraShakeStrength, cameraShakeVibrato, cameraShakeRandomness, cameraShakeFadeout);
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
        UnityEngine.Debug.Log(currentNpcCount);

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


        endOfDayPause = false;
        HandleBaskets(); 
        NpcManager.instance.ResetNpcList();
        hours = 6;
        minutes = 0;
        days++;
        playerInteraction.StopAndStartPlayerMovement(true);
        UpdateTimeData();
        NpcManager.instance.SpawnNpc();
        RenderSettings.skybox = sunriseSkybox;
        StartOfNewDay();
    }

    public void HandleBaskets()
    {
        NpcManager.instance.currentNpcs.Clear();
        NpcManager.instance.currentBaskets.Clear();
    }


    public int ReturnAvailableSpaceOnDeliveryTable()
    {
        int amountAvailable = 0;
        UnityEngine.Debug.Log(deliveryArea.transform.childCount);
        for (int i = 0; i <= deliveryArea.transform.childCount - 2; i++)
        {
            if (!CheckIfObjectHasChild(deliveryArea.transform.GetChild(i)))
            {
                amountAvailable++;
                UnityEngine.Debug.Log(amountAvailable);
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
