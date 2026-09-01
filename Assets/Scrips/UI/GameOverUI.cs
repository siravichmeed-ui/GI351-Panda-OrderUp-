using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Final Score")]
    public TMP_Text finalScoreText;

    [Header("Restart")]
    public Button restartButton;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    public void ShowFinalScore()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameOverUI: ไม่มี GameManager");
            return;
        }

        if (finalScoreText == null)
        {
            Debug.LogError("GameOverUI: ไม่มี FinalScoreText");
            return;
        }

        int finalScore = gameManager.GetScore();

        finalScoreText.text =
            "SCORE : " + finalScore;

        Debug.Log(
            "Final Score: " + finalScore
        );
    }

    void RestartGame()
    {
        if (gameManager != null)
        {
            gameManager.RestartGame();
        }
    }
}