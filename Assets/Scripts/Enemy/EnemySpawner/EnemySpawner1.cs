using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class SpawnOption
{
    public GameObject enemyPrefab; // ★追加

    [Header("出現数")]
    public int spawnCount = 1;
    [Header("確率")]
    public float weight = 1;
    [Header("スポーン遅延時間")]
    public float spawnTime = 0f;
}
public class EnemySpawner1 : MonoBehaviour
{
    public List<SpawnOption> spawnOptions;
    [Header("スポーン範囲")]
    public Vector2 Maxrange = new Vector2(5f,5f);
    public Vector2 Minrange = new Vector2(1f, 1f);
    [Header("シーン上に存在できる敵数")]
    public int maxEnemiesInScene = 10;
    private int currentEnemies = 0;
    [Header("追加で出てくる敵の確率")]
    public float spawnCountOne = 70f;
    public float spawnCountTwo = 20f;
    public float spawnCountThree = 10f;
    private int WaitSpawnCount = 0;
    [Header("ランダム間隔")]
    public float Mindelay = 0.1f;
    public float Maxdelay = 1.0f;
    [Header("確認用")]
    public float delay = 0f;
    private bool isSpawning = false;
    public void OnEnemyDeath(Enemy enemy)
    {
        currentEnemies--;
        
        int count = GetSpawnCount();

        WaitSpawnCount += count;

        delay = Random.Range(Mindelay, Maxdelay);
        if (!isSpawning)
        {
            StartCoroutine(SpawnAfterDelay(delay));
        }
    }

    private void Start()
    {
        SpawnFirst();
    }

    // 追加で何体出すか
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

    //スポーンと抽選処理

    //最初に呼び出す
    void SpawnFirst()
    {
        SpawnOption option = GetRandomOption();

        for (int i = 0; i < option.spawnCount; i++)
        {
            Vector3 pos = transform.position;
            //円形状に
            //Vector2 offset = Random.insideUnitCircle * Maxrange.y;
            //pos += (Vector3)offset;
            //ランダムに
            pos.x += Random.Range(-Minrange.x, Maxrange.x);
            pos.y += Random.Range(-Minrange.y, Minrange.y);
            pos.z = 0f;

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();

            Register(enemy);
        }
    }
    //通常
    //IEnumerator SpawnRoutine(int count)
    //{
    //
    //    while(true)
    //    {
    //        Debug.Log("Spawn呼ばれたよ！！");
    //
    //        if (currentEnemies < maxEnemiesInScene) 
    //        {
    //            SpawnOption option = GetRandomOption();//種類抽選
    //            for (int i = 0; i < option.spawnCount; i++)
    //            {
    //                if (currentEnemies >= maxEnemiesInScene) break;
    //                Vector3 pos = transform.position;
    //                pos.x += Random.Range(-Minrange.x, Maxrange.x);
    //                pos.y += Random.Range(-Minrange.y, Minrange.y);
    //                pos.z = 0f;
    //
    //                GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);
    //
    //                Enemy enemy = obj.GetComponent<Enemy>();//敵を登録
    //                enemy.spawnTime = option.spawnTime;
    //
    //                Register(enemy);
    //            }
    //            //次の種類まで待つ
    //            yield return new WaitForSeconds(option.spawnTime);
    //        }
    //        else
    //        {
    //            // 敵が多いときは少し待つ
    //            yield return new WaitForSeconds(0.5f);
    //        }
    //        
    //    }
    //    
    //}

    //敵時間分待つ(敵が死んだ時に呼び出す)
    IEnumerator SpawnAfterDelay(float delay)
    {
        isSpawning = true;

        yield return new WaitForSeconds(delay);

        int spawnCount = WaitSpawnCount;
        WaitSpawnCount = 0;

        SpawnOption option = GetRandomOption();

        for (int i = 0; i < spawnCount; i++)
        {
            if (currentEnemies >= maxEnemiesInScene)
            {
                WaitSpawnCount += (spawnCount - i);
                yield break;
            }

            Vector3 pos = transform.position;

            Vector2 offset = Random.insideUnitCircle * Maxrange.y;
            pos += (Vector3)offset;

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();

            Register(enemy);

            yield return new WaitForSeconds(0.2f);
        }
        isSpawning = false;

        if(WaitSpawnCount > 0)
        {
            float newDelay = Random.Range(Mindelay, Maxdelay);
            StartCoroutine(SpawnAfterDelay(newDelay));
        }
    }
    public void Register(Enemy enemy)
    {
        currentEnemies++;
        enemy.OnDeath += OnEnemyDeath;
    }
}