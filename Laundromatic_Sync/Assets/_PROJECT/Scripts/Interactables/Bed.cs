using _PROJECT._Jacques.Scripts.Interfaces;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : MonoBehaviour, Iinteractable
{
    [SerializeField] GameObject canvas;
    [SerializeField] StudioEventEmitter morningSound;

    public void Interact()
    {
        if (DayNight.instance.hours >= 22 || DayNight.instance.hours <= 2)
        {
            DayNight.instance.EndDay();
            morningSound.Play();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            PlayerSettings.instance.playerInteraction.StopAndStartPlayerMovement();
            canvas.SetActive(true);
            Debug.Log($"[Bed] Ask Player if they are sure");
        }

    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerSettings.instance.playerInteraction.StopAndStartPlayerMovement();
    }

    public void EndDayEarly()
    {
        morningSound.Play();
        DayNight.instance.EndDay();
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
