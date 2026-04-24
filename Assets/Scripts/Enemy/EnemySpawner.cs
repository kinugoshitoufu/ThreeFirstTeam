using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("スポーン設定")]
    public GameObject enemyPrefab;
    public float spawnChance = 0.5f;
    public float spawnWidth = 4f;

    [Header("複数スポーン")]
    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;

    [Header("制限")]
    public int maxEnemies = 10;
    private int currentEnemies = 0;

    public void RegisterEnemy(Enemy enemy)
    {
        currentEnemies++;
        enemy.OnDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        currentEnemies--;

        // 上限なら何もしない
        if (currentEnemies >= maxEnemies)
            return;

        // ★ここが「確率で追加生成」の本体
        if (Random.value <= spawnChance)
        {
            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
            SpawnMultiple(spawnCount);
        }
    }

    private void SpawnMultiple(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 最大数チェック
            if (currentEnemies >= maxEnemies)
                return;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 pos = transform.position;

        float offsetX = Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
        pos.x += offsetX;

        GameObject obj = Instantiate(enemyPrefab, pos, Quaternion.identity);

        Enemy enemy = obj.GetComponent<Enemy>();
        RegisterEnemy(enemy);
    }
}
