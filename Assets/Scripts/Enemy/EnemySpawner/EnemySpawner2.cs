using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnOption
{
    public GameObject enemyPrefab;

    [Header("出現数")]
    public int spawnCount = 1;

    [Header("確率")]
    public float weight = 1;
}

public enum AreaType
{
    LeftTop,    //左上
    Top,        //上
    RightTop,   //右上

    Left,       //左
    Center,     //中央
    Right,      //右

    LeftBottom, //左下
    Bottom,     //下
    RightBottom //右下
}

public class EnemySpawner2 : MonoBehaviour
{
    [SerializeField] Transform player;

    public List<EnemySpawnOption> spawnOptions;

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

    private int linespawnCount = 0;
    private AreaType beforeArea;
    public static bool StartSpawnFlag = false;
    [Header("敵同士の距離")]
    public float enemyCheckRadius = 1.0f;
    [Header("敵レイヤー")]
    public LayerMask enemyLayer;

    //敵がその位置にいるかを確認
    bool CanSpawn(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapCircle(pos,enemyCheckRadius,enemyLayer);
    
        return hit == null;
    }
    //========================================================
    // プレイヤーがどのエリアにいるか
    //========================================================
    AreaType GetPlayerArea()
    {
        Vector3 pos = player.position - transform.position;

        float xLine = (Maxrange.x * 2f) / 3f;
        float yLine = (Maxrange.y * 2f) / 3f;

        // 左列
        if (pos.x < -xLine /2f)
        {
            if (pos.y > yLine /2f)
                return AreaType.LeftTop;

            if (pos.y < -yLine /2f)
                return AreaType.LeftBottom;

            return AreaType.Left;
        }

        // 中列
        else if (pos.x < xLine /2f)
        {
            if (pos.y > yLine /2f)
                return AreaType.Top;

            if (pos.y < -yLine /2f)
                return AreaType.Bottom;

            return AreaType.Center;
        }

        // 右列
        else
        {
            if (pos.y > yLine /2f)
                return AreaType.RightTop;

            if (pos.y < -yLine /2f)
                return AreaType.RightBottom;

            return AreaType.Right;
        }
    }
    AreaType GetSpawnArea()
    {
        AreaType playerArea = GetPlayerArea();

        switch (playerArea)
        {
            case AreaType.LeftTop:
                return AreaType.RightBottom;

            case AreaType.Top:
                return AreaType.Bottom;

            case AreaType.RightTop:
                return AreaType.LeftBottom;

            case AreaType.Left:
                return AreaType.Right;

            case AreaType.Center:

                // 中央なら外周からランダム
                AreaType[] areas =
                {
                AreaType.LeftTop,
                AreaType.Top,
                AreaType.RightTop,
                AreaType.Left,
                AreaType.Right,
                AreaType.LeftBottom,
                AreaType.Bottom,
                AreaType.RightBottom
            };

                return areas[Random.Range(0, areas.Length)];

            case AreaType.Right:
                return AreaType.Left;

            case AreaType.LeftBottom:
                return AreaType.RightTop;

            case AreaType.Bottom:
                return AreaType.Top;

            case AreaType.RightBottom:
                return AreaType.LeftTop;
        }

        return AreaType.Center;
    }
    //========================================================
    // エリア内のランダム位置取得
    //========================================================
    /*
    Vector3 GetSpawnPosition(AreaType area)
    {
        Vector3 pos = transform.position;

        float cellWidth = (Maxrange.x * 2f) / 3f;
        float cellHeight = (Maxrange.y * 2f) / 3f;

        float minX = 0;
        float maxX = 0;

        float minY = 0;
        float maxY = 0;

        switch (area)
        {
            case AreaType.LeftTop:
                minX = -Maxrange.x;
                maxX = -Maxrange.x + cellWidth;

                minY = Maxrange.y - cellHeight;
                maxY = Maxrange.y;
                break;

            case AreaType.Top:
                minX = -cellWidth / 2f;
                maxX = cellWidth / 2f;

                minY = Maxrange.y - cellHeight;
                maxY = Maxrange.y;
                break;

            case AreaType.RightTop:
                minX = Maxrange.x - cellWidth;
                maxX = Maxrange.x;

                minY = Maxrange.y - cellHeight;
                maxY = Maxrange.y;
                break;

            case AreaType.Left:
                minX = -Maxrange.x;
                maxX = -Maxrange.x + cellWidth;

                minY = -cellHeight / 2f;
                maxY = cellHeight / 2f;
                break;

            case AreaType.Center:
                minX = -cellWidth / 2f;
                maxX = cellWidth / 2f;

                minY = -cellHeight / 2f;
                maxY = cellHeight / 2f;
                break;

            case AreaType.Right:
                minX = Maxrange.x - cellWidth;
                maxX = Maxrange.x;

                minY = -cellHeight / 2f;
                maxY = cellHeight / 2f;
                break;

            case AreaType.LeftBottom:
                minX = -Maxrange.x;
                maxX = -Maxrange.x + cellWidth;

                minY = -Maxrange.y;
                maxY = -Maxrange.y + cellHeight;
                break;

            case AreaType.Bottom:
                minX = -cellWidth / 2f;
                maxX = cellWidth / 2f;

                minY = -Maxrange.y;
                maxY = -Maxrange.y + cellHeight;
                break;

            case AreaType.RightBottom:
                minX = Maxrange.x - cellWidth;
                maxX = Maxrange.x;

                minY = -Maxrange.y;
                maxY = -Maxrange.y + cellHeight;
                break;
        }

        pos.x += Random.Range(minX, maxX);
        pos.y += Random.Range(minY, maxY);

        return pos;
    }
    */
    Vector3 GetLineSpawnPosition(AreaType area)
    {
        Vector3 start = Vector3.zero;
        Vector3 middle = Vector3.zero;
        Vector3 end = Vector3.zero;

        switch (area)
        {
            case AreaType.Left:

                // 右から左へ
                start = new Vector3(Maxrange.x, Maxrange.y, 0);
                middle = new Vector3(Maxrange.x, 0, 0);
                end = new Vector3(Maxrange.x, -Maxrange.y, 0);

                break;

            case AreaType.Right:

                // 左から右へ
                start = new Vector3(-Maxrange.x, Maxrange.y, 0);
                middle = new Vector3(-Maxrange.x, 0, 0);
                end = new Vector3(-Maxrange.x, -Maxrange.y, 0);

                break;

            case AreaType.Top:

                // 下から上へ
                start = new Vector3(-Maxrange.x, -Maxrange.y, 0);
                middle = new Vector3(0, -Maxrange.y, 0);
                end = new Vector3(Maxrange.x, -Maxrange.y, 0);

                break;

            case AreaType.Bottom:

                // 上から下へ
                start = new Vector3(-Maxrange.x, Maxrange.y, 0);
                middle = new Vector3(0, Maxrange.y, 0);
                end = new Vector3(Maxrange.x, Maxrange.y, 0);

                break;
        }

        Vector3 pos;

        // 0～6
        if (linespawnCount < 7)
        {
            float t = linespawnCount / 6f;

            pos = Vector3.Lerp(start, middle, t);
        }
        else
        {
            // 7～13
            float t = (linespawnCount - 7) / 6f;

            pos = Vector3.Lerp(middle, end, t);
        }

        linespawnCount++;

        if (linespawnCount >= 14)
        {
            linespawnCount = 0;
        }

        return transform.position + pos;
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
    EnemySpawnOption GetRandomOption()
    {
        if(spawnOptions.Count <= 0)
        {
            Debug.Log("spawnOptionsに何も入ってないです");
            return null;
        }

        AreaType area = GetPlayerArea();

        List<EnemySpawnOption> candidates = new List<EnemySpawnOption>();

        switch (area)
        {
            // 中央にいる時はショット敵
            case AreaType.Center:

                foreach (var opt in spawnOptions)
                {
                    if (opt.enemyPrefab.GetComponent<EnemyShot>())candidates.Add(opt);
                }

            break;

            // 左上にいる時
            case AreaType.LeftTop:

                foreach (var opt in spawnOptions)
                {
                    if (!opt.enemyPrefab.GetComponent<EnemyShot>())
                    {
                        return opt;
                    }
                }

                break;
        }

        // 通常抽選
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

    //========================================================
    // 初回スポーン
    //========================================================
    void Update()
    {
        if (StartSpawnFlag)SpawnFirst();
        AreaType currentArea = GetPlayerArea();

        //エリア変わったらリセット
        if (currentArea != beforeArea)
        {
            linespawnCount = 0;
            beforeArea = currentArea;
        }
    }

    void SpawnFirst()
    {
        Debug.Log("SpawnFirst");

        if (!StartSpawnFlag)
            return;

        Debug.Log("スポーン開始");

        EnemySpawnOption option = GetRandomOption();

        AreaType area = GetSpawnArea();

        for (int i = 0; i < option.spawnCount; i++)
        {
            Vector3 pos = GetLineSpawnPosition(area);

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

            EnemySpawnOption option = GetRandomOption();

            AreaType area = GetSpawnArea();

            Vector3 pos = GetLineSpawnPosition(area);

            // 敵がいるなら別位置を探す
            int tryCount = 0;

            while (!CanSpawn(pos) && tryCount < 20)
            {
                pos = GetLineSpawnPosition(area);
                tryCount++;
            }

            // 20回試しても無理ならスキップ
            if (tryCount >= 20)
            {
                continue;
            }

            GameObject obj =
                Instantiate(option.enemyPrefab, pos, Quaternion.identity);

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

        StartSpawnFlag = flag;
    }
}