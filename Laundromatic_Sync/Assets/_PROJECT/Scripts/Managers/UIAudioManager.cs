using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class UIAudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0f, 1f)]
    public float mastervolume = 1f;
    [Range(0f, 1f)]
    public float musicvolume = 1f;
    [Range(0f, 1f)]
    public float SFXvolume = 1f;
    [Range(0f, 1f)]
    public float Ambiencevolume = 1f;
    [Range(0f, 1f)]
    public float UIvolume = 1f;
    [Range(0f, 1f)]

    private Bus masterBus;
    private Bus musicBus;
    private Bus SFXBus;
    private Bus AmbienceBus;
    private Bus UIBus;

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

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        AmbienceBus = RuntimeManager.GetBus("bus:/Ambience");
        UIBus = RuntimeManager.GetBus("bus:/UI");

    }

    private void Update()
    {
        //on slider value changed?
        masterBus.setVolume(mastervolume);
        musicBus.setVolume(musicvolume);
        SFXBus.setVolume(SFXvolume);
        AmbienceBus.setVolume(Ambiencevolume);
        UIBus.setVolume(UIvolume);


    }

    public void PlayButtonClick()
    {
        RuntimeManager.PlayOneShot(ButtonClick, 0);
    }
}
