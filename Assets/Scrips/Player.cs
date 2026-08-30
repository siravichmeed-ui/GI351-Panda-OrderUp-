using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Gravity")]
    public float gravity = 3f;

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
            // Gravity กลับด้าน
            rb.gravityScale = -gravity;
        }
        else
        {
            // Gravity ปกติ
            rb.gravityScale = gravity;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public bool IsOnTop()
    {
        return onTop;
    }


}
