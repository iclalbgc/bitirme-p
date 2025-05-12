// PositionHandler.cs
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    private void Awake()
    {
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        carLapCounters = carLapCounterArray.ToList();

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
            lapCounter.OnPassCheckpoint += OnPassCheckpoint;

            // Her zaman pozisyon yazısını görünür yap
            if (lapCounter.carPositionText != null)
            {
                lapCounter.carPositionText.gameObject.SetActive(true);
            }
        }

        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
    }

    void Start()
    {
        leaderboardUIHandler.InitializeList(carLapCounters);
    }

    void OnPassCheckpoint(CarLapCounter _)
    {
        carLapCounters = carLapCounters
            .OrderByDescending(s => s.GetScore())
            .ThenBy(s => s.GetTimeAtLastCheckPoint())
            .ToList();

        for (int i = 0; i < carLapCounters.Count; i++)
        {
            carLapCounters[i].SetCarPosition(i + 1);
        }

        leaderboardUIHandler.UpdateList(carLapCounters);
    }
}
