using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<Wave> waveList;       // ウェーブリスト
    [SerializeField, Min(0)] int waveIndex = 0; // ウェーブ番号

    List<Wave> cloneList = new List<Wave>();    // クローンリスト
    int cloneIndex = 0;                         // クローン番号
    bool listEnd = false;                       // 終了フラグ

    void Start()
    {
        StartWave(waveIndex);
    }

    void Update()
    {
        // ウェーブの削除確認
        for (int i = 0; i < cloneList.Count; ++i)
        {
            if (cloneList[i] && cloneList[i].IsDelete())
            {
                Destroy(cloneList[i].gameObject);
            }
        }

        if (waveList.Count <= 0 || IsEnd()) return;

        // ウェーブが終わった
        if (cloneList[cloneIndex].IsEnd())
        {
            // 次のウェーブへ
            NextWave();
        }
    }

    // ウェーブ開始
    void StartWave(int _index)
    {
        cloneList.Add((Wave)Instantiate(waveList[_index]));
    }

    // 次のウェーブへ
    void NextWave()
    {
        if (waveIndex < (waveList.Count - 1))
        {
            ++waveIndex;
            ++cloneIndex;
            StartWave(waveIndex);
        }
        else
        {
            listEnd = true;
        }
    }

    // 全てのウェーブが終了したか
    public bool IsEnd()
    {
        return listEnd;
    }
}
