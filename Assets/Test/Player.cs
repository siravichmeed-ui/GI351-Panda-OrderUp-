using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    public float gravity = 3f;
    public float switchForce = 15f;

    private bool onTop = false;

    void Start()
    {
        rb.gravityScale = gravity;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwitchSide();
        }
    }

    void SwitchSide()
    {
        onTop = !onTop;

        // หยุดความเร็วเดิม
        rb.linearVelocity = Vector2.zero;

        if (onTop)
        {
            // ดึงขึ้นไปหาพื้นด้านบน
            rb.gravityScale = -gravity;
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else
        {
            // ดึงลงไปหาพื้นด้านล่าง
            rb.gravityScale = gravity;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


}
