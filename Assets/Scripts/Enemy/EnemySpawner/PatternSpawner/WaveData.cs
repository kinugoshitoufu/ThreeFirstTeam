using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    [Header("Wave名")]
    public string waveName;

    [Header("出現パターン候補")]
    public List<EnemySpawnOption3> spawnOptions;

    [Header("このWaveで出す回数")]
    public int spawnCount = 5;

    [Header("出現間隔")]
    public float spawnInterval = 1.0f;
}