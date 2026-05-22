using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeScript : MonoBehaviour
{
    public static TimeScript instance;
    public float limitTime = 30f;     // 残り時間
    public float downspeed = 1f;     // 減少速度
    private float elapsedDownTime = 0f;  // 指定時間ごとに減らす経過時間
    private float elapsedTime = 0f;  // ゲーム開始から始まる経過時間
    public float eeduceLimit = 30f;   //減らす時間
    public float speed = 0.1f;  //回転速度
    public TextMeshProUGUI timetext;

    //吉本追加
    public float upTecreaseTime = 0.2f;//○○秒おきに減る時間スピードを早くさせるか
    public GameObject timeUpEffect;

    private bool isFinished = false;
    public static bool stopTimer = false;
    void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {

        // 時間の減少
        if (!stopTimer) limitTime -= Time.deltaTime * downspeed*speed;
        // 30fごとに減少する経過時間を加算
        elapsedDownTime += Time.deltaTime;

        //ゲーム開始からの経過時間
        elapsedTime += Time.deltaTime;

        // 30秒ごとに減少速度を0.1追加
        if (elapsedDownTime >= eeduceLimit)
        {
            downspeed += upTecreaseTime;
            elapsedDownTime = 0f;
            Debug.Log(downspeed.ToString("F1"));
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
        timetext.text = limitTime.ToString("F2");
    }
}
