using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TMP_Text text;

    [Header("การเคลื่อนที่")]
    public float moveSpeed = 50f;

    [Header("เวลาที่แสดง")]
    public float duration = 1f;

    private float timer;

    void Awake()
    {
        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }
    }

    public void Setup(
        string message,
        Color color
    )
    {
        text.text = message;
        text.color = color;

        timer = duration;
    }

    void Update()
    {
        transform.position +=
            Vector3.up *
            moveSpeed *
            Time.deltaTime;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}