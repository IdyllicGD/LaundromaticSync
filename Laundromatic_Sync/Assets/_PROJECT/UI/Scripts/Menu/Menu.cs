using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD;
using FMODUnity;

public class Menu : MonoBehaviour
{
    public GameObject Screen1; //Phone Unlock Screen
    public GameObject Screen2; //Main Screen
    public GameObject Screen3; //Controls Screen
    public Slider menuSlider;
    public float toggleValue = 1f;
    public FMODUnity.EventReference SoundClick;
    public GameObject PauseCanvas;

    private bool hasToggled = false;

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
       if (!hasToggled && Mathf.Approximately(value, toggleValue))
        {
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
