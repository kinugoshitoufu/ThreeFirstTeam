using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave一覧")]
    [SerializeField]
    private List<WaveData> waveList;

    [Header("スポーン範囲")]
    [SerializeField]
    private Vector2 spawnRange = new Vector2(8f, 4f);

    public static bool StartSpawnFlag = false;
    private bool isStarted = false;

    void Update()
    {
        if (StartSpawnFlag && !isStarted)
        {
            Debug.Log("Wave開始許可");
            isStarted = true;

            StartSpawnFlag = false;

            StartCoroutine(StartWaveRoutine());
        }
    }
    IEnumerator StartWaveRoutine()
    {
        foreach (WaveData wave in waveList)
        {
            yield return StartCoroutine(SpawnWave(wave));
        }

        Debug.Log("全Wave終了");
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        Debug.Log($"Wave開始 : {wave.waveName}");

        for (int i = 0; i < wave.spawnCount; i++)
        {
            EnemySpawnOption3 option = GetRandomOption(wave);

            if (option != null)
            {
                Vector3 spawnPos = GetRandomPosition();

                Instantiate(
                    option.patternPrefab,
                    spawnPos,
                    Quaternion.identity);
            }

            yield return new WaitForSeconds(wave.spawnInterval);
        }

        Debug.Log($"Wave終了 : {wave.waveName}");
    }

    EnemySpawnOption3 GetRandomOption(WaveData wave)
    {
        float totalWeight = 0;

        foreach (var option in wave.spawnOptions)
        {
            totalWeight += option.weight;
        }

        float rand = Random.Range(0, totalWeight);

        float current = 0;

        foreach (var option in wave.spawnOptions)
        {
            current += option.weight;

            if (rand <= current)
            {
                return option;
            }
        }

        return null;
    }

    Vector3 GetRandomPosition()
    {
        Camera cam = Camera.main;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float margin = 0.5f;

        float x = Random.Range(
            cam.transform.position.x - halfWidth + margin,
            cam.transform.position.x + halfWidth - margin);

        float y = Random.Range(
            cam.transform.position.y - halfHeight + margin,
            cam.transform.position.y + halfHeight - margin);

        return new Vector3(x, y, 0);
    }

    public static void IsMove(bool flag)
    {
        Debug.Log($"IsMove({flag})");
        if (!StartSpawnFlag)
        {
            StartSpawnFlag = flag;
        }
    
        StartSpawnFlag = flag;
    }
}