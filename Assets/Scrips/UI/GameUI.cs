using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // =====================================================
    // GAME MANAGER
    // =====================================================

    [Header("Game Manager")]
    public GameManager gameManager;


    // =====================================================
    // SCORE
    // =====================================================

    [Header("Score UI")]
    public TMP_Text scoreText;


    // =====================================================
    // HEALTH
    // =====================================================

    [Header("Health UI")]
    public Image healthBar;
    public TMP_Text healthText;


    // =====================================================
    // HEALTH BAR
    // =====================================================

    [Header("Health Bar")]

    public RectTransform healthBarTransform;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        if (healthBarTransform != null)
        {
            // จำตำแหน่งเดิม
            // เผื่อใช้ในอนาคต
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
        UpdateHealth();
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
    // UPDATE HEALTH
    // =====================================================

    void UpdateHealth()
    {
        if (healthBar == null)
            return;

        float health =
            gameManager.GetHealth();

        float maxHealth =
            gameManager.maxHealth;


        // =================================================
        // ป้องกัน maxHealth เป็น 0
        // =================================================

        if (maxHealth <= 0f)
        {
            healthBar.fillAmount = 0f;
            return;
        }


        // =================================================
        // คำนวณเปอร์เซ็นต์
        // =================================================

        float fill =
            health / maxHealth;


        // =================================================
        // จำกัดหลอด 0 - 100%
        // =================================================

        fill =
            Mathf.Clamp01(fill);


        // =================================================
        // อัปเดตหลอด
        // =================================================

        healthBar.fillAmount = fill;


        // =================================================
        // ตัวเลข
        // =================================================

        if (healthText != null)
        {
            healthText.text =
                Mathf.RoundToInt(health) +
                " / " +
                Mathf.RoundToInt(maxHealth);
        }
    }
}