using UnityEngine;
using UnityEngine.UI;

public class SetLeaderboardItemInfo : MonoBehaviour
{
    public Text positionText;
    public Text driverNameText;
    public Text scoreText;

    public void SetPositionText(string newPosition)
    {
        positionText.text = newPosition;
    }

    public void SetDriverNameText(string newDriverName)
    {
        driverNameText.text = newDriverName;
    }

    public void SetScoreText(string newScore)
    {
        scoreText.text = newScore;
    }
}
