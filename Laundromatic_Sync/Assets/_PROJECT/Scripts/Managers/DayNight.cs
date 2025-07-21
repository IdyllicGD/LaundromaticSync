using TMPro;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI minutesText;
    [SerializeField] TextMeshProUGUI hoursText;
    [SerializeField] TextMeshProUGUI dayText;

    [SerializeField]int minutes;
    [SerializeField]int hours;
    [SerializeField]int days;


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
