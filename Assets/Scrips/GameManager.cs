using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // =====================================================
    // HEALTH
    // =====================================================

    [Header("Health")]

    public float startHealth = 100f;

    public float maxHealth = 100f;

    public float burstHealth = 150f;

    private float health;


    // =====================================================
    // HEALTH ลดตามเวลา
    // =====================================================

    [Header("Health ลดตามเวลา")]

    public float healthDecrease = 2f;

    public float decreaseInterval = 1f;

    private float decreaseTimer = 0f;


    // =====================================================
    // IMMORTAL
    // =====================================================

    [Header("ระบบอมตะ")]

    public float immortalDuration = 5f;

    private float immortalTimer = 0f;


    // =====================================================
    // BUBBLE SHIELD
    // =====================================================

    [Header("Bubble Shield")]

    public GameObject bubbleShield;


    // =====================================================
    // SCORE
    // =====================================================

    [Header("Score")]

    private int score = 0;


    // =====================================================
    // PANDA
    // =====================================================

    [Header("Panda")]

    public Transform panda;


    // =====================================================
    // PANDA SCALE
    // =====================================================

    [Header("Panda Scale")]

    public float normalScale = 1f;

    public float maxScale = 1.5f;

    public float burstScale = 2.2f;

    private Vector3 originalPandaScale;


    // =====================================================
    // GAME OVER
    // =====================================================

    [Header("Game Over")]

    public GameObject gameOverPanel;

    private bool gameOver = false;


    // =====================================================
    // BURST
    // =====================================================

    [Header("Panda แตก")]

    public GameObject[] burstFoodPrefabs;

    public int burstFoodCount = 100;

    public float burstForce = 8f;

    public float burstWaitTime = 10f;

    public float burstDelay = 0.2f;

    private bool isBursting = false;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        // ให้เกมเริ่มปกติ
        Time.timeScale = 1f;


        // =================================================
        // HEALTH
        // =================================================

        health = startHealth;

        decreaseTimer = 0f;


        // =================================================
        // IMMORTAL
        // =================================================

        immortalTimer = 0f;


        // =================================================
        // SCORE
        // =================================================

        score = 0;


        // =================================================
        // STATE
        // =================================================

        gameOver = false;

        isBursting = false;


        // =================================================
        // GAME OVER UI
        // =================================================

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }


        // =================================================
        // BUBBLE
        // =================================================

        if (bubbleShield != null)
        {
            bubbleShield.SetActive(false);
        }


        // =================================================
        // PANDA
        // =================================================

        if (panda != null)
        {
            originalPandaScale =
                panda.localScale;

            panda.localScale =
                originalPandaScale *
                normalScale;
        }
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        // เกมจบแล้ว
        if (gameOver)
            return;


        // Panda กำลังแตก
        if (isBursting)
            return;


        // =================================================
        // IMMORTAL TIMER
        // =================================================

        if (immortalTimer > 0f)
        {
            immortalTimer -= Time.deltaTime;


            if (immortalTimer <= 0f)
            {
                immortalTimer = 0f;


                // ปิด Bubble
                if (bubbleShield != null)
                {
                    bubbleShield.SetActive(false);
                }


                Debug.Log(
                    "หมดเวลาอมตะ"
                );
            }
        }


        // =================================================
        // HEALTH ลด
        // =================================================

        if (!IsImmortal())
        {
            UpdateHealthDecrease();
        }


        // =================================================
        // PANDA SCALE
        // =================================================

        UpdatePandaScale();


        // =================================================
        // HEALTH หมด
        // =================================================

        if (health <= 0f)
        {
            health = 0f;

            NormalGameOver();

            return;
        }


        // =================================================
        // HEALTH เกินจุดแตก
        // =================================================

        if (health >= burstHealth)
        {
            StartBurst();

            return;
        }
    }


    // =====================================================
    // HEALTH DECREASE
    // =====================================================

    void UpdateHealthDecrease()
    {
        decreaseTimer +=
            Time.deltaTime;


        if (decreaseTimer >= decreaseInterval)
        {
            decreaseTimer = 0f;


            health -=
                healthDecrease;


            health =
                Mathf.Max(
                    0f,
                    health
                );
        }
    }


    // =====================================================
    // ADD HEALTH
    // =====================================================

    public void AddHealth(float amount)
    {
        if (gameOver)
            return;


        if (isBursting)
            return;


        // =================================================
        // ตอนอมตะ
        //
        // เก็บของได้
        // แต่เลือดไม่เพิ่ม
        // =================================================

        if (IsImmortal())
        {
            Debug.Log(
                "อมตะอยู่ - ไม่เพิ่ม Health"
            );

            return;
        }


        if (amount <= 0f)
            return;


        health += amount;
    }


    // =====================================================
    // DAMAGE HEALTH
    // =====================================================

    public void DamageHealth(float amount)
    {
        if (gameOver)
            return;


        if (isBursting)
            return;


        // =================================================
        // ตอนอมตะ
        //
        // ไม่เสียเลือด
        // =================================================

        if (IsImmortal())
        {
            Debug.Log(
                "อมตะอยู่ - ไม่เสีย Health"
            );

            return;
        }


        if (amount <= 0f)
            return;


        health -= amount;


        health =
            Mathf.Max(
                0f,
                health
            );
    }


    // =====================================================
    // GET HEALTH
    // =====================================================

    public float GetHealth()
    {
        return health;
    }


    // =====================================================
    // IMMORTAL
    // =====================================================

    public void StartImmortal()
    {
        if (gameOver)
            return;


        if (isBursting)
            return;


        // ตั้งเวลาอมตะ
        immortalTimer =
            immortalDuration;


        // =================================================
        // เปิด Bubble
        // =================================================

        if (bubbleShield != null)
        {
            bubbleShield.SetActive(true);
        }


        Debug.Log(
            "IMMORTAL START : " +
            immortalDuration +
            " SEC"
        );
    }


    // =====================================================
    // IS IMMORTAL
    // =====================================================

    public bool IsImmortal()
    {
        return immortalTimer > 0f;
    }


    // =====================================================
    // GET IMMORTAL TIME
    // =====================================================

    public float GetImmortalTime()
    {
        return immortalTimer;
    }


    // =====================================================
    // PANDA SCALE
    // =====================================================

    void UpdatePandaScale()
    {
        if (panda == null)
            return;


        float scale;


        // =================================================
        // 0 - 100
        // =================================================

        if (health <= maxHealth)
        {
            float progress =
                Mathf.Clamp01(
                    health / maxHealth
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
                    maxHealth,
                    burstHealth,
                    health
                );


            scale =
                Mathf.Lerp(
                    maxScale,
                    burstScale,
                    progress
                );
        }


        panda.localScale =
            originalPandaScale *
            scale;
    }


    // =====================================================
    // SCORE
    // =====================================================

    public void AddScore(int amount)
    {
        if (gameOver)
            return;


        if (isBursting)
            return;


        score += amount;
    }


    // =====================================================
    // GET SCORE
    // =====================================================

    public int GetScore()
    {
        return score;
    }


    // =====================================================
    // START BURST
    // =====================================================

    void StartBurst()
    {
        if (gameOver)
            return;


        if (isBursting)
            return;


        isBursting = true;


        Debug.Log(
            "PANDA BURST!"
        );


        // หยุดเกมหลัก
        Time.timeScale = 0f;


        StartCoroutine(
            BurstSequence()
        );
    }


    // =====================================================
    // BURST SEQUENCE
    // =====================================================

    System.Collections.IEnumerator BurstSequence()
    {
        // =================================================
        // รอก่อนแตก
        // =================================================

        yield return new WaitForSecondsRealtime(
            burstDelay
        );


        // =================================================
        // ขยาย Panda
        // =================================================

        if (panda != null)
        {
            panda.localScale =
                originalPandaScale *
                burstScale;
        }


        // =================================================
        // สร้างอาหาร
        // =================================================

        SpawnBurstFood();


        // =================================================
        // รออาหาร
        // =================================================

        yield return new WaitForSecondsRealtime(
            burstWaitTime
        );


        // =================================================
        // GAME OVER
        // =================================================

        BurstGameOver();
    }


    // =====================================================
    // SPAWN BURST FOOD
    // =====================================================

    void SpawnBurstFood()
    {
        if (panda == null)
        {
            Debug.LogWarning(
                "ไม่ได้ใส่ Panda ใน GameManager"
            );

            return;
        }


        if (
            burstFoodPrefabs == null ||
            burstFoodPrefabs.Length == 0
        )
        {
            Debug.LogWarning(
                "ไม่ได้ใส่ Burst Food Prefab"
            );

            return;
        }


        Vector3 pandaPosition =
            panda.position;


        // =================================================
        // สร้างอาหารจำนวนมาก
        // =================================================

        for (
            int i = 0;
            i < burstFoodCount;
            i++
        )
        {
            int randomIndex =
                Random.Range(
                    0,
                    burstFoodPrefabs.Length
                );


            GameObject prefab =
                burstFoodPrefabs[randomIndex];


            if (prefab == null)
                continue;


            GameObject food =
                Instantiate(
                    prefab,
                    pandaPosition,
                    Quaternion.identity
                );


            // =================================================
            // ปิด MoveLeft
            // =================================================

            MoveLeft moveLeft =
                food.GetComponent<MoveLeft>();


            if (moveLeft != null)
            {
                moveLeft.enabled = false;
            }


            // =================================================
            // BurstFood
            // =================================================

            BurstFood burstFood =
                food.GetComponent<BurstFood>();


            if (burstFood == null)
            {
                burstFood =
                    food.AddComponent<BurstFood>();
            }


            burstFood.force =
                burstForce;


            burstFood.lifeTime =
                burstWaitTime;


            // =================================================
            // สุ่มตำแหน่งเล็กน้อย
            // =================================================

            food.transform.position +=
                new Vector3(
                    Random.Range(
                        -0.2f,
                        0.2f
                    ),
                    Random.Range(
                        -0.2f,
                        0.2f
                    ),
                    0f
                );
        }
    }


    // =====================================================
    // BURST GAME OVER
    // =====================================================

    void BurstGameOver()
    {
        if (gameOver)
            return;


        gameOver = true;


        ShowGameOver();


        Time.timeScale = 0f;
    }


    // =====================================================
    // NORMAL GAME OVER
    // =====================================================

    void NormalGameOver()
    {
        if (gameOver)
            return;


        gameOver = true;


        ShowGameOver();


        Time.timeScale = 0f;
    }


    // =====================================================
    // SHOW GAME OVER
    // =====================================================

    void ShowGameOver()
    {
        if (gameOverPanel == null)
            return;


        gameOverPanel.SetActive(true);


        GameOverUI gameOverUI =
            gameOverPanel.GetComponent<GameOverUI>();


        if (gameOverUI != null)
        {
            gameOverUI.ShowFinalScore();
        }
    }


    // =====================================================
    // IS GAME OVER
    // =====================================================

    public bool IsGameOver()
    {
        return gameOver;
    }


    // =====================================================
    // RESTART
    // =====================================================

    public void RestartGame()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}