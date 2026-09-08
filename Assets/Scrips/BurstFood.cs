using UnityEngine;

public class BurstFood : MonoBehaviour
{
    [Header("แรงกระเด็น")]
    public float force = 8f;

    [Header("แรงโน้มถ่วง")]
    public float gravity = 18f;

    [Header("แรงต้านแนวนอน")]
    public float horizontalDrag = 0.5f;

    [Header("เวลาอยู่ในฉาก")]
    public float lifeTime = 10f;

    [Header("การหมุน")]
    public float rotateSpeed = 180f;


    // =====================================================
    // ตัวแปรภายใน
    // =====================================================

    private Vector2 velocity;

    private float timer = 0f;

    private bool started = false;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        // =================================================
        // สุ่มทิศทาง
        // =================================================

        Vector2 direction =
            Random.insideUnitCircle.normalized;


        // ถ้าสุ่มได้ 0
        if (direction == Vector2.zero)
        {
            direction = Vector2.up;
        }


        // =================================================
        // บังคับให้ของพุ่งขึ้น
        // =================================================

        direction.y =
            Mathf.Abs(direction.y);


        direction.Normalize();


        // =================================================
        // สุ่มแรงของแต่ละชิ้น
        // =================================================

        float randomForce =
            Random.Range(
                force * 0.7f,
                force * 1.3f
            );


        // =================================================
        // กำหนดความเร็วเริ่มต้น
        // =================================================

        velocity =
            direction * randomForce;


        started = true;
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        if (!started)
            return;


        // =================================================
        // สำคัญมาก
        //
        // ใช้ unscaledDeltaTime
        // เพราะ GameManager จะใช้
        //
        // Time.timeScale = 0
        //
        // ทำให้ Time.deltaTime = 0
        // =================================================

        float delta =
            Time.unscaledDeltaTime;


        // =================================================
        // GRAVITY
        // =================================================

        velocity.y -=
            gravity * delta;


        // =================================================
        // แรงต้านแนวนอน
        // =================================================

        velocity.x =
            Mathf.Lerp(
                velocity.x,
                0f,
                horizontalDrag * delta
            );


        // =================================================
        // เคลื่อนที่
        // =================================================

        transform.position +=
            (Vector3)(velocity * delta);


        // =================================================
        // หมุน
        // =================================================

        transform.Rotate(
            0f,
            0f,
            rotateSpeed * delta
        );


        // =================================================
        // TIMER
        // =================================================

        timer += delta;


        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}