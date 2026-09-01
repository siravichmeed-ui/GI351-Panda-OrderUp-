using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Score UI")]
    public TMP_Text scoreText;

    [Header("Time UI")]
    public TMP_Text timeText;

    void Update()
    {
        if (gameManager == null)
            return;

        UpdateScore();
        UpdateTime();
    }

    void UpdateScore()
    {
        if (scoreText == null)
            return;

        scoreText.text =
            gameManager.GetScore().ToString();
    }

    void UpdateTime()
    {
        if (timeText == null)
            return;

        float time =
            gameManager.GetCurrentTime();

        int minutes =
            Mathf.FloorToInt(time / 60f);

        int seconds =
            Mathf.FloorToInt(time % 60f);

        timeText.text =
            minutes.ToString("00") +
            ":" +
            seconds.ToString("00");
    }
}