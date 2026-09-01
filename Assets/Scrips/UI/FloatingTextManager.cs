using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [Header("Prefab")]
    public FloatingText floatingTextPrefab;

    [Header("Container")]
    public Transform floatingTextContainer;

    [Header("ตำแหน่ง +คะแนน")]
    public Transform scorePosition;

    [Header("ตำแหน่ง -เวลา")]
    public Transform timePenaltyPosition;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // แสดง +คะแนน
    // =========================

    public void ShowScore(int score)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogWarning(
                "FloatingTextManager: " +
                "ยังไม่ได้ใส่ FloatingText Prefab"
            );

            return;
        }

        if (scorePosition == null)
        {
            Debug.LogWarning(
                "FloatingTextManager: " +
                "ยังไม่ได้ใส่ Score Position"
            );

            return;
        }

        FloatingText floatingText =
            Instantiate(
                floatingTextPrefab,
                scorePosition.position,
                Quaternion.identity,
                floatingTextContainer
            );

        floatingText.Setup(
            "+" + score,
            Color.green
        );
    }

    // =========================
    // แสดง -เวลา
    // =========================

    public void ShowTimePenalty(float amount)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogWarning(
                "FloatingTextManager: " +
                "ยังไม่ได้ใส่ FloatingText Prefab"
            );

            return;
        }

        if (timePenaltyPosition == null)
        {
            Debug.LogWarning(
                "FloatingTextManager: " +
                "ยังไม่ได้ใส่ Time Penalty Position"
            );

            return;
        }

        FloatingText floatingText =
            Instantiate(
                floatingTextPrefab,
                timePenaltyPosition.position,
                Quaternion.identity,
                floatingTextContainer
            );

        floatingText.Setup(
            "-" + amount.ToString("0"),
            Color.red
        );
    }
}