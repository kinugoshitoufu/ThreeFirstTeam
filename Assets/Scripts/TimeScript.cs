using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeScript : MonoBehaviour
{
    public float LimitTime = 30f;        // 残り時間
    public float Downspeed = 1f;        // 減少速度
    private float elapsedTime = 0f; // 経過時間
    private float ReduceLimit=30f;   //減らす時間

    public TMP_Text timeText;

    void Update()
    {
        // 時間の減少
        LimitTime -= Time.deltaTime*Downspeed;

        // 経過時間を加算
        elapsedTime += Time.deltaTime;

        // 30秒ごとに減少速度を0.1追加
        if (elapsedTime >= ReduceLimit)
        {
            Downspeed += 0.1f;
            elapsedTime = 0f;
            Debug.Log(Downspeed.ToString("F1"));
        }
        if(LimitTime < 0f) 
        {
            LimitTime = 0f; 
        }

        // 整数表示（秒）
        timeText.text = Mathf.FloorToInt(LimitTime).ToString("F0");
    }
}
