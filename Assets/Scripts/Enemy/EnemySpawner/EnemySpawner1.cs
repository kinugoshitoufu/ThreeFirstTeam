using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnOption
{
    public GameObject enemyPrefab; // ★追加

    [Header("出現数")]
    public int spawnCount = 1;
    [Header("確率")]
    public float weight = 1;
    [Header("スポーン範囲")]
    public Vector2 spawnRangeX = new Vector2(-2f, 2f);
    public Vector2 spawnRangeY = new Vector2(-2f, 2f);
}
public class EnemySpawner1 : MonoBehaviour
{
    public List<SpawnOption> spawnOptions;

    public float spawnWidth = 4f;

    public int maxEnemiesInScene = 10;
    private static int currentEnemies = 0;

    public float spawnCountOne = 70f;
    public float spawnCountTwo = 20f;
    public float spawnCountThree = 10f;


    private void Start()
    {
        Spawn(GetSpawnCount());
    }
    public void OnEnemyDeath(Enemy enemy)
    {
        currentEnemies--;

        int count = GetSpawnCount();
        Spawn(count);
    }

    // ★何体出すか
    int GetSpawnCount()
    {
        float total = spawnCountOne + spawnCountTwo + spawnCountThree;

        float rand = Random.Range(0, total);

        if (rand < spawnCountOne) return 1;
        if (rand < spawnCountOne + spawnCountTwo) return 2;
        return 3;
    }

    // ★種類はここで抽選（追尾 or ショット）
    SpawnOption GetRandomOption()
    {
        Debug.Log("敵の抽選が開始したよ！！");
        float total = 0f;

        foreach (var opt in spawnOptions)
            total += opt.weight;

        float rand = Random.Range(0, total);

        float current = 0f;

        foreach (var opt in spawnOptions)
        {
            current += opt.weight;
            if (rand <= current)
                return opt;
        }

        return spawnOptions[0];
    }

    void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Debug.Log("Spawn呼ばれたよ！！");

            if (currentEnemies >= maxEnemiesInScene)
                break;

            SpawnOption option = GetRandomOption();

            Vector3 pos = transform.position;
            pos.x += Random.Range(-2f, 2f);

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();
            
            Register(enemy);
        }
    }

    public void Register(Enemy enemy)
    {
        currentEnemies++;
        enemy.OnDeath += OnEnemyDeath;
    }
}