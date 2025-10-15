using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD;
using FMODUnity;
using DG.Tweening;
using System.Data;
using Unity.VisualScripting;
using Unity.AppUI.UI;
using UnityEngine.Rendering;

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

    //BG Scrolling
    public float scrollspeed = 0.1f;
    public RawImage rawImage;
    private Vector2 uvoffset = Vector2.zero;

    //FMOD Event References
    public FMODUnity.EventReference SoundClick; //button click sound

    //CAM Texture
    public RawImage playerCamImage;
    public RenderTexture camTextureON;
    public RenderTexture camTextureOFF;



    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (menuSlider != null)
        { 
            menuSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        
    }
    private void Update()
    {
        uvoffset.x += scrollspeed * Time.deltaTime;
        uvoffset.y += scrollspeed * Time.deltaTime; //scroll image
        //rawImage.uvRect = new Rect(uvoffset, rawImage.uvRect.size);
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
        Time.timeScale = 1f;
    }
    public void StartUpTime()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ToggleCanvas()
    {
        DOTween.Init();
        Screen2.SetActive(true);
        Screen3.SetActive(false);
        PauseCanvas.SetActive(false);
        Time.timeScale = toggleValue;
        Cursor.lockState = CursorLockMode.Locked;

        /*button animations

        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            if (button.CompareTag("AnimateMe")) // Optional filter
            {
                RectTransform rect = button.transform as RectTransform;
                rect.DOScale(3f, 1f)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1)
                    .SetUpdate(true);
            }
        } not working */
    }
    void SetCursor()
    {
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnSliderValueChanged(float value)
    {
        CanvasTransform = GetComponent<RectTransform>();
        float upscale = Mathf.Lerp(1, maxScale, value); // zoom in relation to slider value

        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        currentTween = CanvasTransform.DOScale(upscale, zoomDuration); // for smooth slider zoom

        if (!hasToggled && Mathf.Approximately(value, toggleValue))
        {
            CanvasTransform.DOScale(1, 1).SetEase(Ease.InOutBack); // ease back and bounce
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