using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveObj : MonoBehaviour
{
    enum END_TYPE
    {
        NONE = 0,
        EXISTENCE,  // 存在していたらウェーブ終了にならない
    }

    [SerializeField] END_TYPE endType = END_TYPE.NONE;

    // ウェーブを終了していいか
    public bool IsEnd()
    {
        if (endType == END_TYPE.EXISTENCE)
        {
            if (gameObject) { return false; }
        }

        return true;
    }
}