using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnOption3
{
    public GameObject patternPrefab;

    [Header("確率")]
    public float weight = 1;
}

//public enum AreaType1
//{
//    LeftTop,    //左上
//    Top,        //上
//    RightTop,   //右上

//    Left,       //左
//    Center,     //中央
//    Right,      //右

//    LeftBottom, //左下
//    Bottom,     //下
//    RightBottom //右下
//}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform player;

    public List<EnemySpawnOption3> spawnOptions;

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

    //EnemySpawnOption option;
    private int spawnCountInArea = 0;
    private AreaType currentArea;

    [Header("始点の距離調整")]
    public float offset = 2f;

    //敵がその位置にいるかを確認
    bool CanSpawn(Vector3 pos)
    {
        Collider2D hit = Physics2D.OverlapCircle(pos,enemyCheckRadius,enemyLayer);

        if (hit != null)
        {
            Debug.Log(
                $"当たった:{hit.name} 位置:{hit.transform.position}");
        }
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
    /*
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
    */
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
    Vector3 GetLineSpawnPosition()
    {
        AreaType area = GetPlayerArea();

        Vector3 dir = Vector3.zero;

        switch (area)
        {
            case AreaType.LeftTop:
                dir = new Vector3(1, -1, 0).normalized;
                break;

            case AreaType.Top:
                dir = Vector3.down;
                break;

            case AreaType.RightTop:
                dir = new Vector3(-1, -1, 0).normalized;
                break;

            case AreaType.Left:
                dir = Vector3.right;
                break;

            case AreaType.Center:

                // 真ん中は右へ
                dir = Vector3.right;
                break;

            case AreaType.Right:
                dir = Vector3.left;
                break;

            case AreaType.LeftBottom:
                dir = new Vector3(1, 1, 0).normalized;
                break;

            case AreaType.Bottom:
                dir = Vector3.up;
                break;

            case AreaType.RightBottom:
                dir = new Vector3(-1, 1, 0).normalized;
                break;
        }

        // プレイヤーから少し離す
        Vector3 start = player.position + dir * 2f;

        // ライン長さ
        float lineLength = 8f;

        Vector3 end = start + dir * lineLength;

        Vector3 pos;

        if (spawnCountInArea < 7)
        {
            float t = spawnCountInArea / 6f;

            Vector3 middle = Vector3.Lerp(start, end, 0.5f);

            pos = Vector3.Lerp(start, middle, t);
        }
        else
        {
            float t = (spawnCountInArea - 7) / 6f;

            Vector3 middle =
                Vector3.Lerp(start, end, 0.5f);

            pos = Vector3.Lerp(middle, end, t);
        }

        spawnCountInArea++;

        if (spawnCountInArea >= 14)
        {
            spawnCountInArea = 0;
        }
        //画面制限
        Camera cam = Camera.main;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float margin = 0.5f;

        pos.x = Mathf.Clamp(
            pos.x,
            cam.transform.position.x - halfWidth + margin,
            cam.transform.position.x + halfWidth - margin);

        pos.y = Mathf.Clamp(
            pos.y,
            cam.transform.position.y - halfHeight + margin,
            cam.transform.position.y + halfHeight - margin);
        Debug.Log($"Area:{area} Count:{spawnCountInArea} Pos:{pos}");
        return pos;
    }

    //void NextLinePoint()
    //{
    //    linespawnCount++;
    //
    //    if (linespawnCount >= 14)
    //    {
    //        linespawnCount = 0;
    //    }
    //}
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
    EnemySpawnOption3 GetRandomOption()
    {
        if(spawnOptions.Count <= 0)
        {
            Debug.Log("spawnOptionsに何も入ってないです");
            return null;
        }

        AreaType area = GetPlayerArea();

        List<EnemySpawnOption3> candidates = new List<EnemySpawnOption3>();
        /*
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
        */
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
        if (StartSpawnFlag)
            SpawnFirst();

        AreaType area = GetPlayerArea();

        if (area != currentArea)
        {
            currentArea = area;
            spawnCountInArea = 0;
        }
    }

    void SpawnFirst()
    {
        if (!StartSpawnFlag)return;
        EnemySpawnOption3 option = GetRandomOption();
        if(option == null)
        {
            Debug.LogError("option が null");
            return;
        }
        Debug.Log($"敵:{option.patternPrefab.name}");
        //Debug.Log($"SpawnCount = {option.spawnCount}");
        Debug.Log("スポーン開始");

        //EnemySpawnOption option = spawnOptions[0];
        AreaType area = GetPlayerArea();
        /*
        for (int i = 0; i < option.spawnCount; i++)
        {
            Debug.Log($"i = {i}");
            Vector3 pos = GetLineSpawnPosition();

            GameObject obj = Instantiate(option.enemyPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();

            Register(enemy);
            //NextLinePoint();
        }*/

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
            EnemySpawnOption3 option = GetRandomOption();

            if (option == null)
            {
                Debug.LogError("option が null");
                continue;
            }

            Vector3 pos = GetLineSpawnPosition();
            if (!CanSpawn(pos))
            {
                Debug.Log("敵がいるのでスキップ");
                //NextLinePoint();
                continue;
            }
            GameObject obj = Instantiate(option.patternPrefab, pos, Quaternion.identity);

            Enemy enemy = obj.GetComponent<Enemy>();

            Register(enemy);

            yield return new WaitForSeconds(0.2f);
            /*
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
            */
            //NextLinePoint();
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
    //public static void IsMove(bool flag)
    //{
    //    if (!StartSpawnFlag)
    //    {
    //        StartSpawnFlag = flag;
    //    }
    //
    //    StartSpawnFlag = flag;
    //}
}