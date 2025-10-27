using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum VolumeType
    {
        MASTER,
        MUSIC,
        AMBIENCE,
        UI,
        SFX,
    }
    [Header ("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();

    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                break;
                volumeSlider.value = UIAudioManager.Instance.mastervolume;
            case VolumeType.MUSIC:
                volumeSlider.value = UIAudioManager.Instance.musicvolume;
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = UIAudioManager.Instance.Ambiencevolume;
                break;
            case VolumeType.UI:
                volumeSlider.value = UIAudioManager.Instance.UIvolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = UIAudioManager.Instance.SFXvolume;
                break;


        }

    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                UIAudioManager.Instance.mastervolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                UIAudioManager.Instance.musicvolume = volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                UIAudioManager.Instance.Ambiencevolume = volumeSlider.value;
                break;
            case VolumeType.UI:
                UIAudioManager.Instance.UIvolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                UIAudioManager.Instance.SFXvolume = volumeSlider.value;
                break;


        }
    }

    }
