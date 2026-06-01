using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    [Header("出現パターン")]
    [SerializeField]
    private List<PatternData> patterns;

    [Header("出現間隔")]
    [SerializeField]
    private float spawnInterval = 3f;

    [Header("最大敵数")]
    [SerializeField]
    private int maxEnemyCount = 50;
    [SerializeField]
    private int maxPatternCount = 5;

    public static bool StartSpawnFlag = false;

    private bool isStarted = false;

    void Awake()
    {
        EnemyCounter.KillCount = 0;
    }
    void Update()
    {
        if (StartSpawnFlag && !isStarted)
        {
            isStarted = true;

            StartCoroutine(SpawnLoop());
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log($"[SpawnLoop] 敵数: {enemies.Length} / {maxEnemyCount}");

            if (enemies.Length >= maxEnemyCount) continue;

            SpawnPattern();
        }
    }

    void SpawnPattern()
    {
        PatternData pattern = GetRandomPattern();
        if (pattern == null) return;

        int patternCount = Mathf.Max(1, EnemyCounter.KillCount / 10 + 1);
        if (patternCount > maxPatternCount)
        {
            patternCount = maxPatternCount;
        }
        Debug.Log($"[SpawnPattern] patternCount: {patternCount}");

        for (int i = 0; i < patternCount; i++)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int remainingSlots = maxEnemyCount - enemies.Length;

            // ★ 2つの条件を統合
            if (remainingSlots <= 0 || pattern.enemyCount > remainingSlots)
            {
                Debug.Log($"[SpawnPattern] スポーン中止: 敵数={enemies.Length}, 残りスロット={remainingSlots}, 必要={pattern.enemyCount}");
                break;
            }

            Vector3 pos = GetRandomPosition();
            Instantiate(pattern.patternPrefab, pos, Quaternion.identity);
        }
    }

    PatternData GetRandomPattern()
    {
        float totalWeight = 0f;

        foreach (var p in patterns)
        {
            totalWeight += p.weight;
        }

        float rand = Random.Range(0, totalWeight);

        float current = 0f;

        foreach (var p in patterns)
        {
            current += p.weight;

            if (rand <= current)
            {
                return p;
            }
        }

        return patterns[0];
    }

    Vector3 GetRandomPosition()
    {
        Camera cam = Camera.main;

        float halfHeight = cam.orthographicSize;

        float halfWidth = halfHeight * cam.aspect;

        float margin = 1f;

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
        StartSpawnFlag = flag;
    }

}