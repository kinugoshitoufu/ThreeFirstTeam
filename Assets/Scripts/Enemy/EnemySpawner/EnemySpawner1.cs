using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class SpawnOption
{
    public GameObject enemyPrefab; // ★追加

    [Header("出現数")]
    public int spawnCount = 1;
    [Header("確率")]
    public float weight = 1;
    //[Header("スポーン遅延時間")]
    //public float spawnTime = 0f;
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

        delay = enemy.spawnTime + Random.Range(Mindelay, Maxdelay);
        if (!isSpawning)
        {
            StartCoroutine(SpawnAfterDelay(delay));
        }
    }

    private void Start()
    {
        SpawnFirst();
        // 黒背景の初期透明度
        Color color = backscreen.color;
        color.a = 0f;
        backscreen.color = color;
    }

    // 川本こうせいが追加したコード↓↓↓↓↓
    [Header("最初の敵の関数達")]
    public GameObject StartEnemy;
    public float targetX = 5f;  //目的地
    public float speed = 3f;    //移動速度
    public Image backscreen; //黒背景
    private bool ismove=true;

    void Update()
    {
        FastEmemy();
    }

    void FastEmemy()
    {
        if (StartEnemy == null)
        {
            Debug.Log("StartEnemy死亡!!!!!!!!!!!!");
            Color color = backscreen.color;
            color.a = 0f;
            backscreen.color = color;
            return;
        }
        Debug.Log(ismove);
        if (ismove)
        {
            StartEnemy.transform.position += Vector3.left * 3f * Time.deltaTime;
        }
        // 目的地まで移動
        if (StartEnemy.transform.position.x < targetX)
        {
            ismove = false;
            // 到着したら透明度0.5
            Color color = backscreen.color;
            color.a = 0.5f;
            backscreen.color = color;
        }
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

    //敵がランダムに出てくる(最初だけ)
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

                isSpawning = false;

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