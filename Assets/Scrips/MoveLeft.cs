using UnityEngine;


public class MoveLeft : MonoBehaviour
{
    [Header("การเคลื่อนที่")]
    public float speed = 5f;

    [Header("การหมุน")]
    public float rotationSpeed = 30f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

void FixedUpdate()
{
    float currentSpeed = speed;

    if (GameSpeedManager.Instance != null)
    {
        currentSpeed = GameSpeedManager.Instance.GetSpeed(speed);
    }

    rb.MovePosition(
        rb.position + Vector2.left * currentSpeed * Time.fixedDeltaTime
    );

    transform.Rotate(
        0f,
        0f,
        rotationSpeed * Time.fixedDeltaTime
    );
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        Item item = GetComponent<Item>();

        if (item == null || item.itemData == null)
        {
            Destroy(gameObject);
            return;
        }

        RecipeManager recipeManager =
            FindFirstObjectByType<RecipeManager>();

        if (recipeManager == null)
        {
            Destroy(gameObject);
            return;
        }

        // ของอันตราย
        if (item.itemData.itemType == ItemType.Hazard)
        {
            Debug.Log("โดนของอันตราย: " + item.itemData.itemName);

            /*player.TakeDamage(1);*/

            Destroy(gameObject);
            return;
        }

        // วัตถุดิบ
        recipeManager.CollectItem(item.itemData);

        Destroy(gameObject);
    }
}