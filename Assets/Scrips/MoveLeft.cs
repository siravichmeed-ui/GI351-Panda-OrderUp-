using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Header("การเคลื่อนที่")]

    public float speed = 10f;

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

        randomOffset = Random.Range(0f, 100f);
    }


    // =====================================================
    // MOVE
    // =====================================================

    void FixedUpdate()
    {
        if (collected)
            return;

        float currentSpeed = speed;

        if (GameSpeedManager.Instance != null)
        {
            currentSpeed =
                GameSpeedManager.Instance.GetSpeed(speed);
        }

        float time =
            Time.time + randomOffset;

        float floatOffset =
            Mathf.Sin(time * floatSpeed) * floatHeight;


        if (rb != null)
        {
            Vector2 currentPosition = rb.position;

            float newX =
                currentPosition.x -
                currentSpeed *
                Time.fixedDeltaTime;

            float newY =
                startPosition.y +
                floatOffset;

            rb.MovePosition(
                new Vector2(newX, newY)
            );
        }
        else
        {
            Vector3 position =
                transform.position;

            position.x -=
                currentSpeed *
                Time.fixedDeltaTime;

            position.y =
                startPosition.y +
                floatOffset;

            transform.position = position;
        }
    }


    // =====================================================
    // COLLECT
    // =====================================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected)
            return;

        Player player =
            other.GetComponent<Player>();

        if (player == null)
            return;

        collected = true;


        // =================================================
        // ปิด Collider
        // =================================================

        Collider2D collider =
            GetComponent<Collider2D>();

        if (collider != null)
        {
            collider.enabled = false;
        }


        // =================================================
        // หยุดการเคลื่อนที่
        // =================================================

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }


        // =================================================
        // หา Item
        // =================================================

        Item item =
            GetComponent<Item>();

        if (item == null)
        {
            Destroy(gameObject);
            return;
        }

        if (item.itemData == null)
        {
            Destroy(gameObject);
            return;
        }


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
        // HAZARD
        // =================================================

        if (itemData.itemType == ItemType.Hazard)
        {
            if (gameManager != null &&
                gameManager.IsImmortal())
            {
                Debug.Log(
                    "อมตะอยู่ - Hazard ไม่มีผล"
                );

                PlayPickupSound();

                Destroy(gameObject);

                return;
            }

            HandleHazard(itemData);

            Destroy(gameObject);

            return;
        }


        // =================================================
        // INGREDIENT
        // =================================================

        bool isRequired =
            recipeManager != null &&
            recipeManager.IsRequiredItem(itemData);


        if (isRequired)
        {
            PlayPickupSound();

            recipeManager.CollectItem(itemData);

            Destroy(gameObject);

            return;
        }


        // =================================================
        // ของทั่วไป
        // =================================================

        if (gameManager != null)
        {
            gameManager.AddHealth(
                itemData.healthAmount
            );
        }

        PlayPickupSound();

        Destroy(gameObject);
    }


    // =====================================================
    // HAZARD
    // =====================================================

    void HandleHazard(ItemData itemData)
    {
        GameManager gameManager =
            FindFirstObjectByType<GameManager>();

        if (gameManager == null)
            return;

        float damage =
            itemData.healthAmount;

        gameManager.DamageHealth(damage);
    }


    // =====================================================
    // SOUND
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
}