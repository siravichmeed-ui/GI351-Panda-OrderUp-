using UnityEngine;
using UnityEngine.UI;

public class WarningManager : MonoBehaviour
{
    [Header("UI รูป !")]
    public Image warningImage;

    [Header("ตำแหน่งขอบจอ")]
    public float screenRightOffset = 50f;

    [Header("เวลาเตือน")]
    public float warningDuration = 1f;

    [Header("การกระพริบ")]
    public bool blink = true;
    public float blinkSpeed = 8f;

    private float timer;
    private bool isWarning = false;

    private RectTransform warningRect;

    void Start()
    {
        warningRect = warningImage.GetComponent<RectTransform>();

        warningImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isWarning)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            HideWarning();
            return;
        }

        if (blink)
        {
            float alpha = Mathf.PingPong(
                Time.time * blinkSpeed,
                1f
            );

            Color color = warningImage.color;
            color.a = alpha;
            warningImage.color = color;
        }
    }

    public void ShowWarning(Transform spawnPoint)
    {
        if (spawnPoint == null)
            return;

        isWarning = true;
        timer = warningDuration;

        warningImage.gameObject.SetActive(true);

        // แปลงตำแหน่ง World เป็นตำแหน่ง Screen
        Vector3 screenPosition =
            Camera.main.WorldToScreenPoint(spawnPoint.position);

        // เอาไปไว้ตรงขอบขวาของจอ
        screenPosition.x =
            Screen.width - screenRightOffset;

        warningRect.position = screenPosition;
    }

    void HideWarning()
    {
        isWarning = false;

        warningImage.gameObject.SetActive(false);
    }
}
