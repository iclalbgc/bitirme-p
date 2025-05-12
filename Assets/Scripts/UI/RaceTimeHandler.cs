using UnityEngine;
using UnityEngine.UI;

public class RaceTimeHandler : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] float raceDuration = 60f; // race ends after 60 seconds
    [SerializeField] GameObject raceOverPanel;

    float elapsedTime;
    bool raceEnded = false;

    void Start()
    {
        if (raceOverPanel != null)
            raceOverPanel.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.GetGameState() == GameStates.running && !raceEnded)
        {
            elapsedTime += Time.deltaTime;
            float remaining = raceDuration - elapsedTime;
            remaining = Mathf.Max(0f, remaining);

            int minutes = Mathf.FloorToInt(remaining / 60);
            int seconds = Mathf.FloorToInt(remaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (remaining <= 0f)
            {
                raceEnded = true;
                GameManager.instance.OnRaceOver();
                if (raceOverPanel != null)
                    raceOverPanel.SetActive(true);
            }
        }
    }
}