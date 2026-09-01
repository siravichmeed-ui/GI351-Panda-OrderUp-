using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Header("การเคลื่อนที่")]
    public float speed = 5f;

    [Header("การหมุน")]
    public float rotationSpeed = 30f;

    [Header("เสียง")]
    public AudioClip pickupSound;

    private Rigidbody2D rb;

    // =========================
    // START
    // =========================

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // =========================
    // MOVE
    // =========================

    void FixedUpdate()
    {
        if (rb == null)
            return;

        float currentSpeed = speed;

        // =========================
        // Game Speed
        // =========================

        if (GameSpeedManager.Instance != null)
        {
            currentSpeed =
                GameSpeedManager.Instance.GetSpeed(
                    speed
                );
        }

        // =========================
        // เคลื่อนที่ไปทางซ้าย
        // =========================

        rb.MovePosition(
            rb.position +
            Vector2.left *
            currentSpeed *
            Time.fixedDeltaTime
        );

        // =========================
        // หมุน
        // =========================

        transform.Rotate(
            0f,
            0f,
            rotationSpeed *
            Time.fixedDeltaTime
        );
    }

    // =========================
    // COLLISION WITH PLAYER
    // =========================

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        // =========================
        // เช็กว่าโดน Player หรือไม่
        // =========================

        Player player =
            other.GetComponent<Player>();

        if (player == null)
            return;

        // =========================
        // หา Item
        // =========================

        Item item =
            GetComponent<Item>();

        if (
            item == null ||
            item.itemData == null
        )
        {
            Destroy(gameObject);
            return;
        }

        ItemData itemData =
            item.itemData;

        // =========================
        // HAZARD
        // =========================

        if (
            itemData.itemType ==
            ItemType.Hazard
        )
        {
            HandleHazard(itemData);

            Destroy(gameObject);

            return;
        }

        // =========================
        // INGREDIENT
        // =========================

        if (itemData.itemType == ItemType.Ingredient)
        {   // เล่นเสียงทันทีที่เก็บ
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    pickupSound,
                    transform.position,
                    1f
                );
            }
        
            HandleIngredient(itemData);

            Destroy(gameObject);

            return;
        }

        // =========================
        // ไม่รู้จักประเภท
        // =========================

        Destroy(gameObject);
    }

    // =========================
    // HANDLE HAZARD
    // =========================

    void HandleHazard(ItemData itemData)
    {
        float timePenalty =
            itemData.timePenalty;

        Debug.Log(
            "โดนของอันตราย: " +
            itemData.itemName
        );

        Debug.Log(
            "ลดเวลา: -" +
            timePenalty.ToString("0") +
            " วินาที"
        );

        // =========================
        // หา GameManager
        // =========================

        GameManager gameManager =
            FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            gameManager.DamageTime(
                timePenalty
            );
        }
        else
        {
            Debug.LogWarning(
                "MoveLeft: " +
                "หา GameManager ไม่เจอ"
            );
        }

        // =========================
        // แสดง -เวลา
        // =========================

        if (
            FloatingTextManager.Instance != null
        )
        {
            FloatingTextManager.Instance.ShowTimePenalty(
                timePenalty
            );
        }
        else
        {
            Debug.LogWarning(
                "MoveLeft: " +
                "หา FloatingTextManager ไม่เจอ"
            );
        }
    }

    // =========================
    // HANDLE INGREDIENT
    // =========================

    void HandleIngredient(ItemData itemData)
    {
        // =========================
        // หา RecipeManager
        // =========================

        RecipeManager recipeManager =
            FindFirstObjectByType<RecipeManager>();

        if (recipeManager == null)
        {
            Debug.LogWarning(
                "MoveLeft: " +
                "หา RecipeManager ไม่เจอ"
            );

            return;
        }

        // =========================
        // เก็บวัตถุดิบ
        // =========================

        bool collected =
            recipeManager.CollectItem(
                itemData
            );

        if (collected)
        {
            Debug.Log(
                "เก็บวัตถุดิบ: " +
                itemData.itemName
            );
            
        }
    }
}