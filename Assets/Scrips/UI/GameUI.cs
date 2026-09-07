using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager gameManager;

    // =====================================================
    // SCORE
    // =====================================================

    [Header("Score UI")]
    public TMP_Text scoreText;


    // =====================================================
    // FULLNESS
    // =====================================================

    [Header("Fullness UI")]
    public Image fullnessBar;
    public TMP_Text fullnessText;


    // =====================================================
    // FULLNESS BAR SHAKE
    // =====================================================

    [Header("Fullness Bar Shake")]
    public RectTransform fullnessBarTransform;

    [Tooltip("ความแรงของการสั่น")]
    public float shakeAmount = 4f;

    [Tooltip("ความเร็วของการสั่น")]
    public float shakeSpeed = 25f;

    private Vector2 originalBarPosition;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        if (fullnessBarTransform != null)
        {
            originalBarPosition =
                fullnessBarTransform.anchoredPosition;
        }
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        if (gameManager == null)
            return;

        UpdateScore();
        UpdateFullness();

        // =================================================
        // เช็กว่าความอิ่มเกินหลอดหรือไม่
        // =================================================

        if (gameManager.GetFullness() >
            gameManager.maxFullness)
        {
            ShakeFullnessBar();
        }
        else
        {
            ResetFullnessBar();
        }
    }


    // =====================================================
    // UPDATE SCORE
    // =====================================================

    void UpdateScore()
    {
        if (scoreText == null)
            return;

        scoreText.text =
            ": " +
            gameManager.GetScore().ToString();
    }


    // =====================================================
    // UPDATE FULLNESS
    // =====================================================

    void UpdateFullness()
    {
        if (fullnessBar == null)
            return;

        float fullness =
            gameManager.GetFullness();

        float maxFullness =
            gameManager.maxFullness;

        // =================================================
        // ป้องกัน maxFullness เป็น 0
        // =================================================

        if (maxFullness <= 0f)
        {
            fullnessBar.fillAmount = 0f;
            return;
        }

        // =================================================
        // คำนวณเปอร์เซ็นต์ของหลอด
        // =================================================

        float fill =
            fullness / maxFullness;

        // จำกัดให้อยู่ 0 - 1
        fill =
            Mathf.Clamp01(fill);

        // =================================================
        // อัปเดตหลอด
        // =================================================

        fullnessBar.fillAmount = fill;

        // =================================================
        // อัปเดตตัวเลข
        // =================================================

        if (fullnessText != null)
        {
            fullnessText.text =
                Mathf.RoundToInt(fullness) +
                " / " +
                Mathf.RoundToInt(maxFullness);
        }
    }


    // =====================================================
    // SHAKE FULLNESS BAR
    // =====================================================

    void ShakeFullnessBar()
    {
        if (fullnessBarTransform == null)
            return;

        // ใช้ UnscaledTime
        // เพราะตอน Game Over Time.timeScale จะเป็น 0
        float time =
            Time.unscaledTime;

        float x =
            Mathf.Sin(
                time * shakeSpeed
            ) * shakeAmount;

        float y =
            Mathf.Cos(
                time * shakeSpeed * 1.3f
            ) * shakeAmount;

        fullnessBarTransform.anchoredPosition =
            originalBarPosition +
            new Vector2(x, y);
    }


    // =====================================================
    // RESET FULLNESS BAR
    // =====================================================

    void ResetFullnessBar()
    {
        if (fullnessBarTransform == null)
            return;

        fullnessBarTransform.anchoredPosition =
            originalBarPosition;
    }
}