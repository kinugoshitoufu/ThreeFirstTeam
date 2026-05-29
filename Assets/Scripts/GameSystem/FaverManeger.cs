using UnityEngine;
using UnityEngine.UI;

public class FaverManeger : MonoBehaviour
{
    public int FaverCount = 0;
    public RectTransform GageImage;
    public float StartY = -213f;// ゲージ開始位置
    public float MaxY = 0f;// ゲージMAXになった時の位置
    public  int MaxCount = 15;// フィーバー状態になるコンボ数
    public float FaverTime = 10f;//フィーバー時間
    private float MaxFaverTime;//フィーバー時間
    public static bool IsFaver=false;
    
    void Start()
    {
        MaxFaverTime = FaverTime;
    }

    void Update()
    {
        // 0～1に変換
        float t = FaverCount / (float)MaxCount;

        // 座標計算
        float y = Mathf.Lerp(StartY, MaxY, t);

        // ゲージ移動
        GageImage.anchoredPosition = new Vector2(0, y);

        // フィーバー
        if (FaverCount >= MaxCount)
        {
            IsFaver = true;
            Debug.Log("フィーバー状態");
            FaverTime-= Time.deltaTime;
            if(FaverTime<0)
            {
                // リセット
                FaverTime = MaxFaverTime;
                FaverCount = 0;
                IsFaver = false;
            }
        }
    }
}

