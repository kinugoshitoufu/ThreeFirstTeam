using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // TextMeshProを使用
    public float LimitTime = 10f;
    void Update()
    {
        // 経過時間を減算
        LimitTime -= Time.deltaTime;

        // 書式を指定して表示
        timerText.text = "Time: " + LimitTime;

        if(LimitTime<0)
        {
            
        }
    }
}
