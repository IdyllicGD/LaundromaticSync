using UnityEngine;
using UnityEngine.UI;

public class UIButtonSFX : MonoBehaviour
{
    private Button button;
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (UIAudioManager.Instance != null)
            UIAudioManager.Instance.PlayButtonClick();
    }
}
