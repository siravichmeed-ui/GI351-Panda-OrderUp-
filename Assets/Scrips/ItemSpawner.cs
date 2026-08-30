using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("ของที่จะ Spawn")]
    public GameObject[] spawnObjects;

    [Header("จุด Spawn")]
    public Transform[] spawnPoints;

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
        // 0 - 10 วินาที
        if (gameTime < earlyTime)
        {
            return earlySpawnInterval;
        }

        // 10 - 20 วินาที
        if (gameTime < midTime)
        {
            return midSpawnInterval;
        }

        // 20+ วินาที
        return overloadSpawnInterval;
    }

    void SpawnObject()
    {
        if (spawnObjects.Length == 0 || spawnPoints.Length == 0)
            return;

        int objectIndex = Random.Range(0, spawnObjects.Length);
        int pointIndex = Random.Range(0, spawnPoints.Length);

        Transform point = spawnPoints[pointIndex];

        Instantiate(
            spawnObjects[objectIndex],
            point.position,
            point.rotation
        );
    }
}

