using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnOption2
{
    public GameObject enemyPrefab;

    [Header("出現数")]
    public int spawnCount = 1;

    [Header("確率")]
    public float weight = 1;
}

public class EnemySpawner3 : MonoBehaviour
{
    [SerializeField] PlayerScript player;

    public List<EnemySpawnOption2> spawnOptions;

    [Header("スポーン範囲")]
    public Vector2 Maxrange = new Vector2(9f, 9f);

    [Header("シーン上に存在できる敵数")]
    public int maxEnemiesInScene = 10;

    private int currentEnemies = 0;

    [Header("追加で出てくる敵の確率")]
    public float spawnCountOne = 70f;
    public float spawnCountTwo = 20f;
    public float spawnCountThree = 10f;

    private int WaitSpawnCount = 0;

    [Header("ランダム間隔")]
    public float Maxdelay = 1.0f;
    public float Mindelay = 0.1f;

    private bool isSpawning = false;

    public static bool SpawnFlag = false;
    public static bool StartSpawnFlag = false;

    Vector3 GetForwardSpawnPosition()
    {
        Vector3 playerPos = player.transform.position;

        Vector3 dir = player.Arrow.transform.up;

        // プレイヤーからどれくらい離すか
        float distance = 6f;

        Vector3 ramdomOffset = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0f);

        Vector3 spawnPos = playerPos + dir * distance + ramdomOffset;

        return spawnPos;
    }

    //========================================================
    // 出現数抽選
    //========================================================
    int GetSpawnCount()
    {
        float total = spawnCountOne + spawnCountTwo + spawnCountThree;

        float rand = Random.Range(0, total);

        if (rand < spawnCountOne)
            return 1;

        if (rand < spawnCountOne + spawnCountTwo)
            return 2;

        return 3;
    }

    //========================================================
    // 敵抽選
    //========================================================
    EnemySpawnOption2 GetRandomOption()
    {
        // 通常抽選
        float total = 0f;

        foreach (var opt in spawnOptions)total += opt.weight;

        float rand = Random.Range(0, total);

        float current = 0f;

        foreach (var opt in spawnOptions)
        {
            current += opt.weight;

            if (rand <= current)return opt;
        }

        return spawnOptions[0];
    }

    //========================================================
    // 初回スポーン
    //========================================================
    void Update()
    {
        SpawnFirst();
    }

    void SpawnFirst()
    {
        Debug.Log("SpawnFirst");

        if (!StartSpawnFlag)
            return;

        Debug.Log("スポーン開始");

        EnemySpawnOption2 option = GetRandomOption();

        for (int i = 0; i < option.spawnCount; i++)
        {
            Vector3 pos = GetForwardSpawnPosition();

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();

            Register(enemy);
        }

        StartSpawnFlag = false;

        StartCoroutine(SpawnAfterDelay(1f));
    }

    //========================================================
    // 死亡後スポーン
    //========================================================
    public void OnEnemyDeath(Enemy enemy)
    {
        currentEnemies--;

        if (StartSpawnFlag)
            return;

        int count = GetSpawnCount();

        WaitSpawnCount += count;

        float delay =
            enemy.spawnTime + Random.Range(Mindelay, Maxdelay);

        if (!isSpawning)
        {
            StartCoroutine(SpawnAfterDelay(delay));
        }
    }

    IEnumerator SpawnAfterDelay(float delay)
    {
        isSpawning = true;

        yield return new WaitForSeconds(delay);

        int spawnCount = WaitSpawnCount;

        WaitSpawnCount = 0;

        for (int i = 0; i < spawnCount; i++)
        {
            if (currentEnemies >= maxEnemiesInScene)
            {
                WaitSpawnCount += (spawnCount - i);

                isSpawning = false;

                yield break;
            }

            EnemySpawnOption2 option = GetRandomOption();

            Vector3 pos = GetForwardSpawnPosition();

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();

            Register(enemy);

            yield return new WaitForSeconds(0.2f);
        }

        isSpawning = false;

        if (WaitSpawnCount > 0)
        {
            float newDelay = Random.Range(Mindelay, Maxdelay);

            StartCoroutine(SpawnAfterDelay(newDelay));
        }
    }

    //========================================================
    // 敵登録
    //========================================================
    public void Register(Enemy enemy)
    {
        currentEnemies++;

        enemy.OnDeath += OnEnemyDeath;
    }

    //========================================================
    // スポーン開始
    //========================================================
    public static void IsMove(bool flag)
    {
        if (!StartSpawnFlag)
        {
            StartSpawnFlag = flag;
        }

        SpawnFlag = flag;
    }
}