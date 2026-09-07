using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("ความอิ่ม")]
    public float maxFullness = 100f;

    [Tooltip("ถ้าความอิ่มถึงค่านี้ Panda จะแตก")]
    public float burstFullness = 150f;

    [SerializeField]
    private float fullness = 0f;

    [Header("Fullness ลดตามเวลา")]
    [Tooltip("ลดความอิ่มกี่หน่วย")]
    public float fullnessDecrease = 2f;

    [Tooltip("ลดทุกกี่วินาที")]
    public float decreaseInterval = 1f;

    private float decreaseTimer = 0f;

    [Header("คะแนน")]
    [SerializeField]
    private int score = 0;

    [Header("Panda")]
    public Transform panda;

    [Header("ขนาด Panda")]
    public float normalScale = 1f;
    public float maxScale = 1.5f;
    public float burstScale = 2.2f;

    private Vector3 originalPandaScale;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    private bool gameOver = false;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        // เริ่มเกมด้วยความอิ่ม 0
        fullness = 0f;

        // เริ่มคะแนน 0
        score = 0;

        // Reset timer
        decreaseTimer = 0f;

        // ปิด Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // จำขนาด Panda เดิม
        if (panda != null)
        {
            originalPandaScale = panda.localScale;

            panda.localScale =
                originalPandaScale * normalScale;
        }
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        if (gameOver)
            return;

        // ลดความอิ่มตลอดเวลา
        UpdateFullnessDecrease();

        // ทำให้ Panda อ้วนตามความอิ่ม
        UpdatePandaScale();

        // ถ้าความอิ่มถึงค่าที่กำหนด
        // ให้ Panda แตก
        if (fullness >= burstFullness)
        {
            GameOver();
        }
    }


    // =====================================================
    // FULLNESS เพิ่ม
    // =====================================================

    public void AddFullness(float amount)
    {
        if (gameOver)
            return;

        if (amount <= 0f)
            return;

        fullness += amount;

        Debug.Log(
            "Fullness +" +
            amount +
            " | ตอนนี้ = " +
            fullness
        );
    }


    // =====================================================
    // FULLNESS ลดตามเวลา
    // =====================================================

    void UpdateFullnessDecrease()
    {
        decreaseTimer += Time.deltaTime;

        if (decreaseTimer >= decreaseInterval)
        {
            decreaseTimer = 0f;

            fullness -= fullnessDecrease;

            // ไม่ให้ติดลบ
            fullness = Mathf.Max(
                0f,
                fullness
            );

            Debug.Log(
                "Fullness -" +
                fullnessDecrease +
                " | ตอนนี้ = " +
                fullness
            );
        }
    }


    // =====================================================
    // GET FULLNESS
    // =====================================================

    public float GetFullness()
    {
        return fullness;
    }


    // =====================================================
    // PANDA อ้วน
    // =====================================================

    void UpdatePandaScale()
    {
        if (panda == null)
            return;

        float scale;


        // =================================================
        // 0 - 100
        // =================================================

        if (fullness <= maxFullness)
        {
            float progress =
                Mathf.Clamp01(
                    fullness / maxFullness
                );

            scale =
                Mathf.Lerp(
                    normalScale,
                    maxScale,
                    progress
                );
        }


        // =================================================
        // 100 - 150
        // =================================================

        else
        {
            float progress =
                Mathf.InverseLerp(
                    maxFullness,
                    burstFullness,
                    fullness
                );

            scale =
                Mathf.Lerp(
                    maxScale,
                    burstScale,
                    progress
                );
        }


        // =================================================
        // เปลี่ยนขนาด Panda
        // =================================================

        panda.localScale =
            originalPandaScale * scale;
    }


    // =====================================================
    // SCORE
    // =====================================================

    public void AddScore(int amount)
    {
        if (gameOver)
            return;

        score += amount;

        Debug.Log(
            "Score +" +
            amount +
            " | Score = " +
            score
        );
    }


    // =====================================================
    // GET SCORE
    // =====================================================

    public int GetScore()
    {
        return score;
    }


    // =====================================================
    // GAME OVER
    // =====================================================

    void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;

        Debug.Log(
            "PANDA แตก!"
        );

        Debug.Log(
            "Fullness = " +
            fullness
        );

        Debug.Log(
            "Final Score = " +
            score
        );

        // เปิด Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            GameOverUI gameOverUI =
                gameOverPanel.GetComponent<GameOverUI>();

            if (gameOverUI != null)
            {
                gameOverUI.ShowFinalScore();
            }
        }

        // หยุดเกม
        Time.timeScale = 0f;
    }


    // =====================================================
    // CHECK GAME OVER
    // =====================================================

    public bool IsGameOver()
    {
        return gameOver;
    }


    // =====================================================
    // RESTART GAME
    // =====================================================

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}