using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public Transform object1;
    public Transform object2;

    public float speed = 5f;

    // ขยับซ้อนกันนิดเดียวเพื่อป้องกันรอยต่อ
    public float overlap = 0.02f;

    private SpriteRenderer sprite1;
    private SpriteRenderer sprite2;

    void Start()
    {
        sprite1 = object1.GetComponent<SpriteRenderer>();
        sprite2 = object2.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // เลื่อนทั้งสอง object ไปทางซ้าย
        object1.position += Vector3.left * speed * Time.deltaTime;
        object2.position += Vector3.left * speed * Time.deltaTime;

        // ขอบซ้ายของกล้อง
        float screenLeft = Camera.main.ViewportToWorldPoint(
            new Vector3(0f, 0f, 0f)
        ).x;

        // Object 1 หลุดจอหมดแล้ว
        if (sprite1.bounds.max.x <= screenLeft)
        {
            object1.position = new Vector3(
                sprite2.bounds.max.x
                + sprite1.bounds.extents.x
                - overlap,

                object1.position.y,
                object1.position.z
            );
        }

        // Object 2 หลุดจอหมดแล้ว
        if (sprite2.bounds.max.x <= screenLeft)
        {
            object2.position = new Vector3(
                sprite1.bounds.max.x
                + sprite2.bounds.extents.x
                - overlap,

                object2.position.y,
                object2.position.z
            );
        }
    }
}