using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

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
    [SerializeField] Texture2D sunriseSkybox;
    [SerializeField] Texture2D eveningSkybox;
    [SerializeField] Texture2D nightSkybox;
    [SerializeField] Texture2D sunsetSkybox;
    [Title("Delivery", null, TitleAlignments.Centered)]
    [SerializeField] GameObject deliveryArea;
    public int delivryAreaMaxAmount;


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
        StartCoroutine(TimeToWaitForStart(1f));
    }
    public void CheckForMail()
    {

    }

    void Update()
    {
        if (endOfDayPause) return;
        tempSeconds += Time.deltaTime;
        if (tempSeconds >= 1)
        {
            minutes++;
            if (minutes >= 60)
            {
                hours++;
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

    public void PauseTimeAndCloseStore()
    {
        NpcManager.instance.canSpawnNpc = false;
    }

    public void EndDay()
    {
        int currentNpcCount = NpcManager.instance.currentNpcs.Count;
        Debug.Log(currentNpcCount);
        for (int i = 0; i < currentNpcCount; i++)
        {
            Destroy(NpcManager.instance.currentNpcs[i].gameObject);
        }


        NpcManager.instance.currentNpcs.Clear();
        hours = 6;
        minutes = 0;
        days++;
        PlayerSettings.instance.playerInteraction.StopAndStartPlayerMovement(true);
        UpdateTimeData();
        NpcManager.instance.SpawnNpc();
        StartOfDayDelivery();
    }

    void StartOfDayDelivery()
    {
        int spawnerOrders = 0;
        for (int i = 0; i < deliveryArea.transform.childCount; i++)
        {
            if (spawnerOrders < ShopManager.instance.orderList.Count && !CheckIfObjectHasChild(deliveryArea.transform.GetChild(i)))
            {
                Instantiate(ShopManager.instance.orderList[spawnerOrders], deliveryArea.transform.GetChild(i));
                spawnerOrders++;
            }
            Debug.Log($"[DayNight] {i}");

            
            
            


        }
        StartCoroutine(TimeToWait(5f));
        ShopManager.instance.orderList.Clear();

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
    IEnumerator TimeToWaitForStart(float time)
    {
        yield return new WaitForSeconds(time);
        StartOfDayDelivery();
        
    }

    
}
