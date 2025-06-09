using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD;
using FMODUnity;
using DG.Tweening;
using System.Data;

public class Menu : MonoBehaviour
{
    public GameObject Screen1; //Phone Unlock Screen
    public GameObject Screen2; //Main Screen
    public GameObject Screen3; //Controls Screen

    //Canvas'
    public GameObject PauseCanvas;

    //Intro Slider Behaviour
    public Slider menuSlider;
    private float toggleValue = 1f; //Slider complete value
    private bool hasToggled = false;
    private float maxScale = 1.25f; //Canvas zoomed in scale
    private float zoomDuration = .1f;
    private RectTransform CanvasTransform;
    private Tween currentTween;

    //FMOD Event References
    public FMODUnity.EventReference SoundClick; //button click sound
    

    

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (menuSlider != null)
        { 
            menuSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void QuitGame ()
    {
        UnityEngine.Debug.Log("Quit");
        Application.Quit();
    }

    public void ReturnToMenu ()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleCanvas ()
    {
        Screen2.SetActive(true);
        Screen3.SetActive(false);
        PauseCanvas.SetActive(false);
        Time.timeScale = toggleValue;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void SetCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnSliderValueChanged(float value)
    {
        CanvasTransform = GetComponent<RectTransform>();
        float upscale = Mathf.Lerp(1, maxScale, value);

        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        currentTween = CanvasTransform.DOScale(upscale, zoomDuration);

        if (!hasToggled && Mathf.Approximately(value, toggleValue))
        {
            CanvasTransform.DOScale(1, 1).SetEase(Ease.InOutBack);
            Screen1.SetActive(false);
            Screen2.SetActive(true);   
            hasToggled = true;
            FMODUnity.RuntimeManager.PlayOneShot(SoundClick, 0);
        }
    
    }

    public void ToggleControls()
    {
        Screen2.SetActive(false);
        Screen3.SetActive(true);
    }

    public void ToggleMain()
    {
        Screen3.SetActive(false);
        Screen2.SetActive(true);
    }
    
    public void ButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(SoundClick, 0);
    }
}
