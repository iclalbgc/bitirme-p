using System;
using UnityEngine;
using UnityEngine.UI;

public class CarLapCounter : MonoBehaviour
{
    public Text carPositionText;
    public Text carScoreText;

    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;
    int numberOfPassedCheckpoints = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 2;
    bool isRaceCompleted = false;
    int carPosition = 0;

    int score = 0;

    public event Action<CarLapCounter> OnPassCheckpoint;

    public void AddScore(int amount)
    {
        score += amount;
        if (carScoreText != null)
            carScoreText.text = score.ToString();
    }

    public int GetScore() => score;

    public void SetCarPosition(int position)
    {
        carPosition = position;
        carPositionText.text = carPosition.ToString();
        carPositionText.gameObject.SetActive(true);
    }

    public int GetNumberOfCheckpointsPassed() => numberOfPassedCheckpoints;
    public float GetTimeAtLastCheckPoint() => timeAtLastPassedCheckPoint;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("CheckPoint"))
        {
            if (isRaceCompleted) return;
            CheckPoint checkPoint = collider2D.GetComponent<CheckPoint>();
            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassedCheckPoint = Time.time;

                AddScore(50);

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;
                    if (lapsCompleted >= lapsToComplete)
                        isRaceCompleted = true;
                }

                OnPassCheckpoint?.Invoke(this);
                carPositionText.text = carPosition.ToString();
                carPositionText.gameObject.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        if (TryGetComponent<OverDriveDriftController>(out var driftController))
        {
            if (driftController.IsTireScreeching(out float lateralVel, out bool isBraking))
            {
                if (lateralVel > 1f)
                {
                    AddScore(Mathf.RoundToInt(lateralVel * 0.5f));
                }
            }
        }
    }
}