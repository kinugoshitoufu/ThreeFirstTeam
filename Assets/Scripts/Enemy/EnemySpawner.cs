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
}
public class EnemySpawner : MonoBehaviour
{
    [Header("スポーン設定")]
    public List<SpawnOption> spawnOptions = new List<SpawnOption>();
    public float spawnWidth = 4f;

    [Header("制限")]
    public int maxEnemiesInScene = 10;
    private static int currentEnemiesInScene = 0;//敵がシーンに何体いるか？
    // =========================
    // 初期スポーンなどで使うなら
    // =========================
    private void Start()
    {
        Spawn(); // 初期1回
    }

    // =========================
    // 敵登録（重要）
    // =========================
    public void RegisterEnemy(Enemy enemy)
    {
        currentEnemiesInScene++;
        enemy.OnDeath += OnEnemyDeath;
    }

    // =========================
    // 死亡通知
    // =========================
    private void OnEnemyDeath(Enemy enemy)
    {
        currentEnemiesInScene--;

        if (currentEnemiesInScene >= maxEnemiesInScene)
            return;

        Spawn();
    }

    // =========================
    // スポーン本体
    // =========================
    public void Spawn()
    {
        SpawnOption option = GetRandomOption();

        for (int i = 0; i < option.spawnCount; i++)
        {
            if (currentEnemiesInScene >= maxEnemiesInScene)
                return;

            Vector3 pos = transform.position;

            float offsetX = Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
            pos.x += offsetX;

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();
            RegisterEnemy(enemy);
        }
    }

    // =========================
    // 重み抽選
    // =========================
    private SpawnOption GetRandomOption()
    {
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

}