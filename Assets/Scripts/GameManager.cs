using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver }

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private GameStates gameState = GameStates.countDown;

    [SerializeField] private float countdownDuration = 3f;
    [SerializeField] private GameObject raceOverPanel;
    [SerializeField] private GameObject settingsPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null)
            {
                bool isActive = settingsPanel.activeSelf;
                settingsPanel.SetActive(!isActive);

                // Oyunu duraklat / devam ettir
                Time.timeScale = isActive ? 1f : 0f;

                // Tüm sesleri duraklat / devam ettir
                ToggleAllAudio(!isActive);
            }
        }

        Debug.Log("ESC kontrolü yapılıyor");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
        settingsPanel = GameObject.Find("SettingsPanel");
    }

    private void LevelStart()
    {
        Time.timeScale = 1f;
        gameState = GameStates.countDown;
        Debug.Log("Level started");
        StartCoroutine(BeginRaceAfterCountdown());
    }

    private IEnumerator BeginRaceAfterCountdown()
    {
        yield return new WaitForSeconds(countdownDuration);
        OnRaceStart();
    }

    public void OnRaceStart()
    {
        Debug.Log("Race Started");
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

    public GameStates GetGameState()
    {
        return gameState;
    }

    private void ToggleAllAudio(bool shouldPlay)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            if (shouldPlay)
                audio.UnPause();
            else
                audio.Pause();
        }
    }
}
