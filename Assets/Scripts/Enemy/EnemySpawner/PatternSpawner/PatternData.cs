using UnityEngine;

[System.Serializable]
public class PatternData
{
    [Header("パターンPrefab")]
    public GameObject patternPrefab;

    [Header("抽選確率")]
    public float weight = 1f;

    [Header("パターンにいる敵の数")]
    public int enemyCount = 3;
}