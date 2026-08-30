
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
        Player player =
            other.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log("จับ Item ได้!");

            Destroy(gameObject);
        }
    }
}

