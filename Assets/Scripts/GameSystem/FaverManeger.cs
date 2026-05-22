using UnityEngine;
using UnityEngine.UI;

public class FaverManeger : MonoBehaviour
{
    public int FaverCount = 0;
    public RectTransform GageImage;
    public float startY = -213f;// ゲージ開始位置
    public float maxY = 0f;// ゲージMAX位置
    public int maxCount = 15;// 最大カウント

    void Update()
    {
        // 0～1に変換
        float t = FaverCount / (float)maxCount;

        // 座標計算
        float y = Mathf.Lerp(startY, maxY, t);

        // ゲージ移動
        GageImage.anchoredPosition = new Vector2(0, y);

        // フィーバー
        if (FaverCount >= maxCount)
        {
            Debug.Log("フィーバー状態!!!!");

            // リセット
            FaverCount = 0;
        }
    }
}