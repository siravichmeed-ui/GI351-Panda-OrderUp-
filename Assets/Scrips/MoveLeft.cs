using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Header("การเคลื่อนที่")]
    public float speed = 5f;

    [Header("การลอยแบบฟองสบู่")]
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;

    [Header("เสียงเก็บ Item")]
    public AudioClip pickupSound;

    private Rigidbody2D rb;

    private Vector2 startPosition;

    private float randomOffset;

    private bool collected = false;

    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            startPosition = rb.position;
        }
        else
        {
            startPosition = transform.position;
        }

        // ทำให้แต่ละ Item ลอยคนละจังหวะ
        randomOffset =
            Random.Range(0f, 100f);
    }

    // =====================================================
    // MOVE
    // =====================================================

    void FixedUpdate()
    {
        if (rb == null)
            return;

        if (collected)
            return;

        // =================================================
        // ความเร็ว
        // =================================================

        float currentSpeed = speed;

        if (GameSpeedManager.Instance != null)
        {
            currentSpeed =
                GameSpeedManager.Instance.GetSpeed(speed);
        }

        // =================================================
        // ลอยขึ้นลง
        // =================================================

        float time =
            Time.time + randomOffset;

        float floatOffset =
            Mathf.Sin(
                time * floatSpeed
            ) * floatHeight;

        // =================================================
        // ตำแหน่งใหม่
        // =================================================

        float newX =
            rb.position.x -
            currentSpeed *
            Time.fixedDeltaTime;

        float newY =
            startPosition.y +
            floatOffset;

        // =================================================
        // เคลื่อนที่
        // =================================================

        rb.MovePosition(
            new Vector2(
                newX,
                newY
            )
        );
    }

    // =====================================================
    // PLAYER COLLECT ITEM
    // =====================================================

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (collected)
            return;

        // =================================================
        // เช็ก Player
        // =================================================

        Player player =
            other.GetComponent<Player>();

        if (player == null)
            return;

        // =================================================
        // หา Item
        // =================================================

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

        collected = true;

        // =================================================
        // ปิด Collider
        // =================================================

        Collider2D col =
            GetComponent<Collider2D>();

        if (col != null)
        {
            col.enabled = false;
        }

        // =================================================
        // หยุด Item
        // =================================================

        if (rb != null)
        {
            rb.linearVelocity =
                Vector2.zero;
        }

        // =================================================
        // ข้อมูล Item
        // =================================================

        ItemData itemData =
            item.itemData;

        // =================================================
        // หา Manager
        // =================================================

        RecipeManager recipeManager =
            FindFirstObjectByType<RecipeManager>();

        GameManager gameManager =
            FindFirstObjectByType<GameManager>();

        // =================================================
        // เช็กว่าเป็นของใน Menu หรือไม่
        // =================================================

        bool isRequired =
            recipeManager != null &&
            recipeManager.IsRequiredItem(itemData);

        // =================================================
        // ของถูกต้องตาม Menu
        // =================================================

        if (isRequired)
        {
            // เล่นเสียง
            PlayPickupSound();

            // ส่งให้ RecipeManager
            recipeManager.CollectItem(
                itemData
            );

            // เล่น Animation ฟองแตก
            PlayBubbleBreak();

            return;
        }

        // =================================================
        // ของไม่ใช่ใน Menu
        // =================================================

        if (gameManager != null)
        {
            gameManager.AddFullness(
                itemData.fullnessAmount
            );
        }

        Debug.Log(
            "เก็บของไม่ตรง Menu: " +
            itemData.itemName +
            " +" +
            itemData.fullnessAmount +
            " Fullness"
        );

        // เล่นเสียง
        PlayPickupSound();

        // เล่น Animation ฟองแตก
        PlayBubbleBreak();
    }

    // =====================================================
    // PICKUP SOUND
    // =====================================================

    void PlayPickupSound()
    {
        if (pickupSound == null)
            return;

        AudioSource.PlayClipAtPoint(
            pickupSound,
            transform.position,
            1f
        );
    }

    // =====================================================
    // BUBBLE BREAK
    // =====================================================

    void PlayBubbleBreak()
    {
        Animator bubbleAnimator =
            GetComponentInChildren<Animator>();

        if (bubbleAnimator != null)
        {
            bubbleAnimator.SetTrigger(
                "Break"
            );

            // รอ Animation ฟองแตก
            Destroy(
                gameObject,
                0.3f
            );
        }
        else
        {
            // ถ้าไม่มี Animator
            // ให้ลบทันที
            Destroy(gameObject);
        }
    }
}