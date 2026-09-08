using UnityEngine;

public class BubbleShield : MonoBehaviour
{
    [Header("ขนาดการเด้ง")]

    public float scaleAmount = 0.05f;


    [Header("ความเร็ว")]

    public float speed = 3f;


    private Vector3 originalScale;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        originalScale =
            transform.localScale;
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        float scale =
            1f +
            Mathf.Sin(
                Time.unscaledTime *
                speed
            ) *
            scaleAmount;


        transform.localScale =
            originalScale *
            scale;
    }
}