using System;
using System.Collections;
using TMPro;
using UnityEngine;
 
public class TimeManager : MonoBehaviour
{
    #region Skybox
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;
 
    [SerializeField] private Gradient graddientNightToSunrise;
    [SerializeField] private Gradient graddientSunriseToDay;
    [SerializeField] private Gradient graddientDayToSunset;
    [SerializeField] private Gradient graddientSunsetToNight;
 
    [SerializeField] private Light globalLight;
    #endregion
    
    #region Visual Timer
    [SerializeField] TextMeshProUGUI minutesText;
    [SerializeField] TextMeshProUGUI hoursText;
    [SerializeField] TextMeshProUGUI dayText;
    #endregion
    
    public static TimeManager instance;
    
    public bool paused = false;
    
    public int minutes;
 
    public int Minutes
    { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }
 
    public int hours;
 
    public int Hours
    { get { return hours; } set { hours = value; OnHoursChange(value); } }
 
    public int days;
 
    public int Days
    { get { return days; } set { days = value; OnDaysChange(value); } }
 
    private float tempSecond;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        minutesText.text = Minutes.ToString("00");
        hoursText.text = Hours.ToString("00");
    }

    public void Update()
    {
        if (paused)return;   
        tempSecond += Time.deltaTime;
 
        if (tempSecond >= 1)
        {
            Minutes += 1;
            minutesText.text = Minutes.ToString("00");
            tempSecond = 0;
        }
    }
 
    private void OnMinutesChange(int value)
    {
        globalLight.transform.Rotate(Vector3.up, (1f / (1440f / 4f)) * 360f, Space.World);
        if (value >= 60)
        {
            Hours++;
            hoursText.text = Hours.ToString("00");
            minutes = 0;
        }
    }
 
    private void OnHoursChange(int value)
    {
        switch (value)
        {
            case 8:
                StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 5f));
                StartCoroutine(LerpLight(graddientNightToSunrise, 5f));
                break;
            case 10:
                StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 5f));
                StartCoroutine(LerpLight(graddientSunriseToDay, 5f));
                break;
            case 17:
                StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 5f));
                StartCoroutine(LerpLight(graddientDayToSunset, 5f));
                break;
            case 20:
                StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 5f));
                StartCoroutine(LerpLight(graddientSunsetToNight, 5f));
                break;
        }
    }

    private void OnDaysChange(int value)
    {
        if (Hours < 24) return;
        Hours = 0;
        paused = true;
        //Days++;
    }
    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }
 
    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}
