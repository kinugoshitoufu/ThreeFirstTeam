using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeScript : MonoBehaviour
{
    public static TimeScript instance;
    public float LimitTime = 30f;     // 残り時間
    public float Downspeed = 1f;     // 減少速度
    private float elapsedTime = 0f;  // 経過時間
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
            LimitTime += 2;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            LimitTime -= 2;
        }

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
        if(LimitTime <= 0f && !isFinished) 
        {
            isFinished = true;

            int finalScore = PlayerScript.instance.GetScore();
            Debug.Log("終了時スコア：" + finalScore);

            ResultScript.resultScore = finalScore;
            ResultScript.resultTime = Mathf.Max(0f, elapsedTime);
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
        redImage.gameObject.SetActive(false);
        yellowImage.gameObject.SetActive(false);
        blueImage.gameObject.SetActive(false);

        // 条件ごとに1つだけON
        if (LimitTime <= 60f)
        {
            redImage.gameObject.SetActive(true);
        }
        else if (LimitTime <= 120f)
        {
            yellowImage.gameObject.SetActive(true);
        }
        else
        {
            blueImage.gameObject.SetActive(true);
        }

    }
}
