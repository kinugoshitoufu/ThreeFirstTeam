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
    private static int currentEnemies = 0;
    [Header("追加で出てくる敵の確率")]
    public float spawnCountOne = 70f;
    public float spawnCountTwo = 20f;
    public float spawnCountThree = 10f;
    public void OnEnemyDeath(Enemy enemy)
    {
        currentEnemies--;
        
        int count = GetSpawnCount();

        //StartCoroutine(SpawnAfterDelay(option, count));
        Spawn(GetSpawnCount());
    }

    private void Start()
    {
        Spawn(GetSpawnCount());
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

    /*単独用
    void Spawn(int count)
    {
        SpawnOption option = GetRandomOption();//種類抽選

        for (int i = 0; i < option.spawnCount; i++)
        {
            Debug.Log("Spawn呼ばれたよ！！");

            if (currentEnemies >= maxEnemiesInScene)
                break;

            Vector3 pos = transform.position;
            pos.x += Random.Range(Maxrange.x, Maxrange.y);
            pos.y += Random.Range(Minrange.x, Minrange.y);
            pos.z = 0f;

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();//敵を登録
          
            Register(enemy);
        }
    }*/

    //コルーチン化
    void Spawn(int count)
    {
        StartCoroutine(SpawnRoutine(count));
    }
    IEnumerator SpawnRoutine(int count)
    {

        while(true)
        {
            Debug.Log("Spawn呼ばれたよ！！");

            if (currentEnemies < maxEnemiesInScene) 
            {
                SpawnOption option = GetRandomOption();//種類抽選
                for (int i = 0; i < option.spawnCount; i++)
                {
                    if (currentEnemies >= maxEnemiesInScene) break;
                    Vector3 pos = transform.position;
                    pos.x += Random.Range(-Minrange.x, Maxrange.x);
                    pos.y += Random.Range(-Minrange.y, Minrange.y);
                    pos.z = 0f;

                    GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

                    Enemy enemy = obj.GetComponent<Enemy>();//敵を登録

                    Register(enemy);
                }
                //次の種類まで待つ
                yield return new WaitForSeconds(option.spawnTime);
            }
            else
            {
                // 敵が多いときは少し待つ
                yield return new WaitForSeconds(0.5f);
            }
            
        }
        
    }

    //IEnumerator SpawnAfterDelay(SpawnOption option, int count)
    //{
    //    yield return new WaitForSeconds(option.spawnDelay);
    //
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (currentEnemies >= maxEnemiesInScene)
    //            yield break;
    //
    //        Vector3 pos = transform.position;
    //        pos.x += Random.Range(Maxrange.x, Maxrange.y);
    //        pos.y += Random.Range(Minrange.x, Minrange.y);
    //        pos.z = 0f;
    //
    //        GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);
    //
    //        Enemy newEnemy = obj.GetComponent<Enemy>();
    //        Register(newEnemy);
    //    }
    //}
    public void Register(Enemy enemy)
    {
        currentEnemies++;
        enemy.OnDeath += OnEnemyDeath;
    }
}