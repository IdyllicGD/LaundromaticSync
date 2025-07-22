using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [Title("Time UI Refrences", null, TitleAlignments.Centered)]
    [SerializeField] TextMeshProUGUI minutesText;
    [SerializeField] TextMeshProUGUI hoursText;
    [SerializeField] TextMeshProUGUI dayText;

    [Title("Current Day Time",null, TitleAlignments.Centered)]
    [SerializeField] int minutes;
    [SerializeField]int hours;
    [SerializeField]int days;

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


    float tempSeconds;

    bool endOfDayPause = false;


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
                }
                hoursText.text = hours.ToString("00");
                minutes = 0;
            }
            minutesText.text = minutes.ToString("00");
            tempSeconds = 0;
        }


    }

    void ChangeMinutes()
    {
        
    }
}
