using _PROJECT._Jacques.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : MonoBehaviour, Iinteractable
{
    public void Interact()
    {
        if (TimeManager.instance.hours >= 22 || TimeManager.instance.hours <= 2)
        {
            SceneManager.LoadScene(0); 
        }
        else
        {
            Debug.Log($"[Bed] Cannot Sleep Yet");
        }
        
    }
}
