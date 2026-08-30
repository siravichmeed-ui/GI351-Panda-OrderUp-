using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Gravity")]
    public float gravity = 3f;

    [Header("Click Delay")]
    public float clickDelay = 0.1f;

    private bool onTop = false;
    private float nextClickTime = 0f;

    void Start()
    {
        rb.gravityScale = gravity;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextClickTime)
        {
            SwitchSide();

            nextClickTime = Time.time + clickDelay;
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
         spriteRenderer.flipY = onTop;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public bool IsOnTop()
    {
        return onTop;
    }


}
