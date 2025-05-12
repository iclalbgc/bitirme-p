// GameManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    GameStates gameState = GameStates.countDown;

    [SerializeField] float countdownDuration = 3f;
    [SerializeField] GameObject raceOverPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void LevelStart()
    {
        Time.timeScale = 1f;
        gameState = GameStates.countDown;
        Debug.Log("Level started");
        StartCoroutine(BeginRaceAfterCountdown());
    }

    IEnumerator BeginRaceAfterCountdown()
    {
        yield return new WaitForSeconds(countdownDuration);
        OnRaceStart();
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");
        gameState = GameStates.running;
        Time.timeScale = 1f;

        if (raceOverPanel != null)
            raceOverPanel.SetActive(false);
    }

    public void OnRaceOver()
    {
        Debug.Log("Race Over");
        gameState = GameStates.raceOver;
        Time.timeScale = 0f;

        CarLapCounter[] cars = FindObjectsOfType<CarLapCounter>();
        foreach (var car in cars)
        {
            car.AddScore(-car.GetScore());

            // SFX durdurma için ses bileşeni varsa kapat
            var sfx = car.GetComponent<CarSfxHandler>();
            if (sfx != null)
            {
                sfx.engineAudioSource.Stop();
                sfx.tiresScreeachingAudioSource.Stop();
            }
        }

        if (raceOverPanel != null)
            raceOverPanel.SetActive(true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }
}
