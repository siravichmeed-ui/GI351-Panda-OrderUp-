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

    [Header("Game Over Sound")]
    public AudioClip gameOverSound;

    [Range(0f, 1f)]
    public float gameOverVolume = 1f;

    private bool soundPlayed = false;


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
        // เล่นเสียง Game Over
        if (!soundPlayed && gameOverSound != null)
        {
            AudioSource.PlayClipAtPoint(
                gameOverSound,
                Camera.main.transform.position,
                gameOverVolume
            );

            soundPlayed = true;
        }

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