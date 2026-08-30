using UnityEngine;


public class MoveLeft : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(
            rb.position + Vector2.left * speed * Time.fixedDeltaTime
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

        // อาวุธ / ของอันตราย
        if (item.itemData.itemType == ItemType.Hazard)
        {
            Debug.Log("โดนของอันตราย: " + item.itemData.itemName);

            /*player.TakeDamage(1);*/

            Destroy(gameObject);

            return;
        }

        // วัตถุดิบ
        recipeManager.CollectItem(item.itemData);

        // Item หายเมื่อชน Player
        Destroy(gameObject);
    }
}