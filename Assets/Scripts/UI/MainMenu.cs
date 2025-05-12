using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start butonu için
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    // Exit butonu için
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapatılıyor..."); // Editor'de test için
    }
}
