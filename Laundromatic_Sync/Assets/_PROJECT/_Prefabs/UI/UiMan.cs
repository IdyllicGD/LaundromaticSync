using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMan : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
