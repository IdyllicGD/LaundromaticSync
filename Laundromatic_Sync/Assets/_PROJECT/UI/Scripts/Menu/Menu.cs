using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Screen1;
    public GameObject Screen2;
    public Slider menuSlider;
    public float toggleValue = 1f;

    private bool hasToggled = false;

    private void Start()
    {
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
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnSliderValueChanged(float value)
    {
       if (!hasToggled && Mathf.Approximately(value, 1f))
        {
            Screen1.SetActive(false);
            Screen2.SetActive(true);   
            hasToggled = true;
        }
    
    }
}
