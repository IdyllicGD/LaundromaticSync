using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD;
using FMODUnity;
using DG.Tweening;
using System.Data;
using Unity.VisualScripting;
using TMPro;
using System.Xml;
using System.Collections;


public class Shop : MonoBehaviour
{

    //canvas
    public Canvas ShopCanvas;
    public Image Background;
    //FMOD Event References
    public FMODUnity.EventReference SoundClick; //button click sound
    //Animation
    public TMP_Text tmpText;
    public float charDelay = 0.05f;



    private void Awake()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        

        StartCoroutine(AnimateText("UPGRADES!")); //IEnumerator animation call
        tmpText.transform.DOLocalMoveY(20f, .5f)    //Additional DOTWeen bounce
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

    }

    IEnumerator AnimateText(string message)
    {
        tmpText.text = ""; // Clear text
        for (int i = 0; i < message.Length; i++)
        {
            string visibleText = tmpText.text;
            string nextChar = message[i].ToString();
            tmpText.text = visibleText + $"<color=#D982D9>{nextChar}"; // Transparent initially
            yield return null;

            tmpText.text = visibleText + $"<color=#D982D9>{nextChar}"; // Opaque white
            yield return new WaitForSeconds(charDelay);
        }
    }
    
    public void ReturnToGame ()
    {
        Time.timeScale = 1f;
        ShopCanvas.enabled = false;
        ButtonClick();
        Cursor.visible = false;
    }
    
    public void BuyWashingMachine()
    {

    }

    public void BuyDryer()
    {

    }

    public void PrevSong()
    {

    }

    public void NextSong()
    {

    }
    public void ButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SoundClick, 0);
    }
}