using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;
    public Text winnerText;
    private List<SetLeaderboardItemInfo> setLeaderboardItemInfo = new List<SetLeaderboardItemInfo>();

    public void InitializeList(List<CarLapCounter> carLapCounters)
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        foreach (Transform child in leaderboardLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        setLeaderboardItemInfo.Clear();

        for (int i = 0; i < carLapCounters.Count; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);
            var info = leaderboardInfoGameObject.GetComponent<SetLeaderboardItemInfo>();
            setLeaderboardItemInfo.Add(info);
        }

        UpdateList(carLapCounters);
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        var sortedLapCounters = lapCounters.OrderByDescending(lc => lc.GetScore()).ToList();

        for (int i = 0; i < sortedLapCounters.Count; i++)
        {
            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
            setLeaderboardItemInfo[i].SetDriverNameText(sortedLapCounters[i].gameObject.name);
            setLeaderboardItemInfo[i].SetScoreText(sortedLapCounters[i].GetScore().ToString());
        }

        if (sortedLapCounters.Count > 0 && winnerText != null)
        {
            string winnerName = sortedLapCounters[0].gameObject.name;
            winnerText.text = $"KAZANAN: {winnerName}";
        }
    }
}
