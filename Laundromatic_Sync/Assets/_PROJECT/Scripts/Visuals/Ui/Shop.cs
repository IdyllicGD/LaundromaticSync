using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD;
using FMODUnity;
using DG.Tweening;
using System.Data;

public class Shop : MonoBehaviour
{

    //canvas
    public Canvas ShopCanvas;
    public Image Background;
    //FMOD Event References
    public FMODUnity.EventReference SoundClick; //button click sound
    



    private void Start()
    {
        ShopCanvas.enabled = false;
    }
    public void ReturnToGame ()
    {
        ShopCanvas.enabled = false;
        ButtonClick();
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