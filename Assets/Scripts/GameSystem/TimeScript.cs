using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeScript : MonoBehaviour
{
    public static TimeScript instance;
    public float LimitTime = 30f;     // 残り時間
    public float Downspeed = 1f;     // 減少速度
    private float ElapsedDownTime = 0f;  // 指定時間ごとに減らす経過時間
    private float ElapsedTime = 0f;  // ゲーム開始から始まる経過時間
    private float ReduceLimit = 30f;   //減らす時間

    public float speed = 0.1f;  //回転速度
    public Image redImage;    //0～60
    public Image yellowImage; //61～120
    public Image blueImage;   //121～180

    public TMP_Text timeText;

    private bool isFinished = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            LimitTime = 130;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            LimitTime = 70;
        }
        if (Input.GetKey(KeyCode.U))
        {
            LimitTime = 2;
        }

        // 時間の減少
        LimitTime -= Time.deltaTime*Downspeed;

        // 30fごとに減少する経過時間を加算
        ElapsedDownTime += Time.deltaTime;

        //ゲーム開始からの経過時間
        ElapsedTime += Time.deltaTime;

        // 30秒ごとに減少速度を0.1追加
        if (ElapsedDownTime >= ReduceLimit)
        {
            Downspeed += 0.1f;
            ElapsedDownTime = 0f;
            Debug.Log(Downspeed.ToString("F1"));
        }

        if(LimitTime <= 0f && !isFinished) 
        {
            isFinished = true;

            int finalScore = PlayerScript.instance.GetScore();
            Debug.Log("終了時スコア：" + finalScore);

            ResultScript.resultScore = finalScore;
            ResultScript.resultTime = Mathf.Max(0f, ElapsedTime);
            ResultScript.resultCombo = PlayerScript.instance.maxcombo;

            SceneChanger.instance.sceneChanger("ResultScene");
            return;
        }
        if (LimitTime < 0)
        {
            LimitTime = 0f;//０以下にならないように
        }

        // 整数表示（秒）
        timeText.text = Mathf.FloorToInt(LimitTime).ToString("F0");

        // 一旦全部OFF
        redImage.gameObject.SetActive(true);
        yellowImage.gameObject.SetActive(true);
        blueImage.gameObject.SetActive(true);
        redImage.fillAmount = 1f;
        yellowImage.fillAmount=1f;
        blueImage.fillAmount =1f;

        if (LimitTime > 120f) // 青180～121
        {
            float t = Mathf.InverseLerp(180f, 121f, LimitTime);
            blueImage.fillAmount = 1f - t;
        }
        else if (LimitTime > 60f) // 黄色120～61
        {
            blueImage.gameObject.SetActive(false);
            float t = Mathf.InverseLerp(120f, 61f, LimitTime);
            yellowImage.fillAmount = 1f - t;
        }
        else // 赤60～0
        {
            blueImage.gameObject.SetActive(false);
            float t = Mathf.InverseLerp(60f, 0f, LimitTime);
            redImage.fillAmount = 1f - t;
        }

    }
}
