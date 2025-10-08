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
            morningSound.Play();
            Cursor.lockState = CursorLockMode.None;
            PlayerSettings.instance.playerInteraction.StopAndStartPlayerMovement();
            canvas.SetActive(true);
            Debug.Log($"[Bed] Ask Player if they are sure");
        

    }

    public void CloseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerSettings.instance.playerInteraction.StopAndStartPlayerMovement();
    }

    public void EndDayEarly()
    {
        morningSound.Play();
        DayNight.instance.EndDayReview();
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }
}
