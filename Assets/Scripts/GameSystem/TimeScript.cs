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
    public Image redImage;    //0～60
    public Image yellowImage; //61～120
    public Image blueImage;   //121～180


    //吉本追加
    public float upTecreaseTime = 0.2f;//○○秒おきに減る時間スピードを早くさせるか
    public GameObject timeUpEffect;

    private bool isResultShown = false;
    private bool isFinished = false;
    public static bool stopTimer = false;
    void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            if (Input.GetKey(KeyCode.K))
            {
                limitTime = 130;
            }
            if (Input.GetKey(KeyCode.Y))
            {
                limitTime = 70;
            }
            if (Input.GetKey(KeyCode.U))
            {
                limitTime = 2;
            }
        }

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

            isResultShown = true;


            int finalScore = PlayerScript.instance.GetScore();
            Debug.Log("終了時スコア：" + finalScore);

            ResultScript.resultScore = finalScore;
            ResultScript.resultTime = Mathf.Max(0f, elapsedTime);
            ResultScript.resultCombo = PlayerScript.instance.maxcombo;
            FadeManager.instance.StartFade();
            ResultScript.instance.ShowResult();
            //PlayerScript.instance.isFalling = true;
            return;
        }
        if (limitTime < 0)
        {
            limitTime = 0f;//０以下にならないように
        }

        // 表示
        redImage.gameObject.SetActive(true);
        yellowImage.gameObject.SetActive(true);
        blueImage.gameObject.SetActive(true);
        redImage.fillAmount = 1f;
        yellowImage.fillAmount = 1f;
        blueImage.fillAmount = 1f;

        if (limitTime > 180f)
        {
            limitTime = 180f;//120以上行かないように
        }
        else if (limitTime > 120f) // 青180～121
        {           
            float t = Mathf.InverseLerp(180f, 121f, limitTime);
            blueImage.fillAmount = 1f - t;
        }
        else if (limitTime > 60f) // 黄色120～61
        {
            yellowImage.gameObject.SetActive(true);
            blueImage.gameObject.SetActive(false);
            float t = Mathf.InverseLerp(120f, 61f, limitTime);
            yellowImage.fillAmount = 1f - t;
        }
        else // 赤60～0
        {
            blueImage.gameObject.SetActive(false);
            yellowImage.gameObject.SetActive(false);
            float t = Mathf.InverseLerp(60f, 0f, limitTime);
            redImage.fillAmount = 1f - t;
        }

    }

    public void UpEffect()
    {
        Vector3 screenPos = redImage.rectTransform.position;

        Vector3 worldPos =Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
        worldPos.z = 0;

        Instantiate(timeUpEffect, worldPos, Quaternion.identity);
    }
}
