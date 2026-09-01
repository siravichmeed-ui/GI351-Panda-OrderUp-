using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("เวลาเกม")]
    public float startingTime = 120f;

    [Header("คะแนนเริ่มต้น")]
    public int score = 0;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    private float currentTime;
    private bool gameOver = false;

    void Start()
    {
        currentTime = startingTime;

        // ปิด Game Over ตอนเริ่ม
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (gameOver)
            return;

        // เวลาเดิน
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;

            GameOver();
        }
    }

    // =========================
    // SCORE
    // =========================

    public void AddScore(int amount)
    {
        if (gameOver)
            return;

        score += amount;

        Debug.Log(
            "ได้รับคะแนน +" +
            amount
        );

        Debug.Log(
            "คะแนนรวม: " +
            score
        );
    }

    public int GetScore()
    {
        return score;
    }

    // =========================
    // TIME
    // =========================

    public void DamageTime(float penalty)
    {
        if (gameOver)
            return;

        currentTime -= penalty;

        if (currentTime < 0f)
        {
            currentTime = 0f;
        }

        Debug.Log(
            "โดน Hazard! เวลาถูกหัก " +
            penalty +
            " วินาที"
        );

        Debug.Log(
            "เวลาที่เหลือ: " +
            currentTime.ToString("F1")
        );

        if (currentTime <= 0f)
        {
            GameOver();
        }
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    // =========================
    // GAME OVER
    // =========================

    void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;

        Debug.Log("GAME OVER");
        Debug.Log("คะแนนสุดท้าย: " + score);

        // เปิด Game Over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            GameOverUI gameOverUI =
                gameOverPanel.GetComponent<GameOverUI>();

            if (gameOverUI != null)
            {
                gameOverUI.ShowFinalScore();
            }
            else
            {
                Debug.LogError(
                    "GameOverPanel ไม่มี GameOverUI"
                );
            }
        }

        // หยุดเกม
        Time.timeScale = 0f;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    // =========================
    // RESTART
    // =========================

    public void RestartGame()
    {
        // คืนเวลาให้เกมก่อน
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}