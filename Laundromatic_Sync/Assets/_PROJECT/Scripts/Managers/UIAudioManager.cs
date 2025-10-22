using UnityEngine;
using FMODUnity;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;
    public FMODUnity.EventReference ButtonClick; //button click sound
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonClick()
    {
        RuntimeManager.PlayOneShot(ButtonClick, 0);
    }
}
