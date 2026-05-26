using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeScript : MonoBehaviour
{
    public static TimeScript instance;
    public float limitTime = 30f;// 残り時間
    public float downspeed = 1f;// 減少速度
    private float elapsedDownTime = 0f; // 指定時間ごとに減らす経過時間
    private float elapsedTime = 0f;  // ゲーム開始から始まる経過時間
    public float eeduceLimit = 30f;   //減らす時間
    public float speed = 0.1f;  //回転速度
    public TextMeshProUGUI timetext;//時間のテキスト
    public Image RedFlashing;//時間がギリギリの時に出てくる赤いやつ
    public float TimeToRed = 10f;//赤く点滅させる残り時間帯
    public float RedTime = 2f;
    public float FlashingSpeed = 5f;//点滅させる速度
    public float FlashDuration = 2f;// 点滅する時間
    private float flashTimer;
    private bool isFlashing;//点滅中か判断するフラグ
    private bool wasUnderLimit = false;

    //吉本追加
    public float upTecreaseTime = 0.2f;//○○秒おきに減る時間スピードを早くさせるか
    public GameObject timeUpEffect;

    private bool isFinished = false;
    public static bool stopTimer = false;
    void Start()
    {
        Time.timeScale = 1;
        RedFlashing.enabled = false;
    }
    void Update()
    {
        // 時間の減少
        if (!stopTimer) limitTime -= Time.deltaTime * downspeed * speed;

        // 30fごとに減少する経過時間を加算
        elapsedDownTime += Time.deltaTime;

        //ゲーム開始からの経過時間
        elapsedTime += Time.deltaTime;

        // 30秒ごとに減少速度を0.1追加
        if (elapsedDownTime >= eeduceLimit)
        {
            downspeed += upTecreaseTime;
            elapsedDownTime = 0f;
        }

        // 今の状態
        bool isUnderLimit = limitTime < TimeToRed;
        // 条件を超えた瞬間に点滅開始
        if (isUnderLimit && !wasUnderLimit && !isFlashing)
        {
            Debug.Log("点滅開始");
            isFlashing = true;
            flashTimer = FlashDuration;
            RedFlashing.enabled = true;
        }

        // 点滅中
        if (isFlashing)
        {
            Debug.Log("点滅中");
            flashTimer -= Time.deltaTime;
            // アルファ点滅
            float alpha = Mathf.PingPong(Time.time * FlashingSpeed, 0.5f);

            Color color = RedFlashing.color;
            color.a = alpha;
            RedFlashing.color = color;

            // 指定時間経過
            if (flashTimer <= 0f)
            {
                Debug.Log(flashTimer);
                Debug.Log("点滅終わり");
                isFlashing = false;

                // 非表示
                RedFlashing.enabled = false;
            }
        }


        if (limitTime <= 0f && !isFinished)
        {
            isFinished = true;

            int finalScore = PlayerScript.instance.GetScore();
            Debug.Log("終了時スコア：" + finalScore);

            ResultScript.resultScore = finalScore;
            ResultScript.resultTime = Mathf.Max(0f, elapsedTime);
            ResultScript.resultCombo = PlayerScript.instance.maxcombo;
            FadeManager.instance.StartFade();
            ResultScript.instance.ShowResult();
            return;
        }
        if (limitTime < 0)
        {
            limitTime = 0f;//０以下にならないように
        }
        timetext.text = limitTime.ToString("F1");
        // 最後に状態保存
        wasUnderLimit = isUnderLimit;
    }
}
