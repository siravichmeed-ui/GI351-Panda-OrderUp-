using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("ของที่จะ Spawn")]
    public GameObject[] spawnObjects;

    [Header("จุด Spawn")]
    public Transform[] spawnPoints;

    [Header("Warning")]
    public WarningManager warningManager;

    [Header("ช่วงเริ่มต้น")]
    public float earlySpawnInterval = 2.5f;

    [Header("ช่วงกลาง")]
    public float midSpawnInterval = 1.2f;

    [Header("ช่วง Overload")]
    public float overloadSpawnInterval = 0.5f;

    [Header("เวลา")]
    public float earlyTime = 10f;
    public float midTime = 20f;

    private float timer;
    private float gameTime;

    void Update()
    {
        gameTime += Time.deltaTime;
        timer += Time.deltaTime;

        float currentSpawnInterval = GetSpawnInterval();

        if (timer >= currentSpawnInterval)
        {
            SpawnObject();
            timer = 0f;
        }
    }

    float GetSpawnInterval()
    {
        if (gameTime < earlyTime)
        {
            return earlySpawnInterval;
        }

        if (gameTime < midTime)
        {
            return midSpawnInterval;
        }

        return overloadSpawnInterval;
    }

    void SpawnObject()
    {
        if (spawnObjects.Length == 0 || spawnPoints.Length == 0)
            return;

        int objectIndex = Random.Range(0, spawnObjects.Length);
        int pointIndex = Random.Range(0, spawnPoints.Length);

        GameObject objectToSpawn = spawnObjects[objectIndex];

        Transform point = spawnPoints[pointIndex];

        Instantiate(
            objectToSpawn,
            point.position,
            point.rotation
        );

        // เช็กว่า Item ที่กำลัง Spawn เป็น Hazard หรือไม่
        Item item = objectToSpawn.GetComponent<Item>();

        if (item != null && item.itemData != null)
        {
            if (item.itemData.itemType == ItemType.Hazard)
            {
                if (warningManager != null)
                {
                    warningManager.ShowWarning(point);
                }
            }
        }
    }
}

