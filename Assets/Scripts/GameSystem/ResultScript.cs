using TMPro;
using UnityEngine;

public class ResultScript : MonoBehaviour
{
    public static ResultScript instance;
    [Header("テキスト")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    
    public static float resultTime;    //リザルト用
    public static int resultScore;     //リザルト用
    public static int resultCombo;        //リザルト用
    [Header("プレハブ入れてね！")]
    public GameObject resultPrefab;
    public GameObject triangle;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timeText.text = resultTime.ToString("F0") + "sec";
        scoreText.text = resultScore.ToString() + "point";
        comboText.text = resultCombo.ToString() + "combo";
    }

    
    void Update()
    {
        
    }
    public void ShowResult()
    {
        GameObject resultObj = Instantiate(resultPrefab);

        TriangleMesh.instance.resultPanel = resultObj.transform.Find("Panel").GetComponent<SpriteRenderer>();
        triangle.SetActive(true);
    }
}
