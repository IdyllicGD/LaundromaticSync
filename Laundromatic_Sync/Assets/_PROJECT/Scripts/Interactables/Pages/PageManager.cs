using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [Title("Pages info")]
    [SerializeField] int amountOfMoneyNeededToWin;
    [SerializeField] TextMeshProUGUI moneyMadeText;
    [SerializeField] TextMeshProUGUI moneyStillNeeded;


    public void CheckData()
    {
        int totalMoneyMade = DayNight.instance.moneyMadeTotal;
        int moneyStillNeeded = amountOfMoneyNeededToWin - totalMoneyMade;
        moneyMadeText.text = totalMoneyMade.ToString();

        if (amountOfMoneyNeededToWin < totalMoneyMade)
        {
            WinState();
        }

    }

    public void WinState()
    {

    }
    
    public void LoseState()
    {
        
    }

}
