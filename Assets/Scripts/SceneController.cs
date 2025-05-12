using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Son leveldesin, başka level yok.");
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Bu sahne build settings'te olmalı
    }

    public void ExitGame()
    {
        Debug.Log("Oyun kapatılıyor...");
        Application.Quit();
    }
}
