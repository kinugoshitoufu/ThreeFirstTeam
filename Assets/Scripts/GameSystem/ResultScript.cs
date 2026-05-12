using TMPro;
using UnityEngine;

public class ResultScript : MonoBehaviour
{
    public static ResultScript instance;
    [Header("テキスト")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI totalText;
    
    public static float resultTime;    //リザルト用
    public static int resultScore;     //リザルト用
    public static int resultCombo;        //リザルト用
    public int TotalScore;
    [Header("プレハブ入れてね！")]
    public GameObject resultPrefab;
    public GameObject triangle;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        TotalScore = (int)resultTime + resultScore + resultCombo;
        timeText.text = "Life Time:" + resultTime.ToString("F0");
        scoreText.text = "Score:" + resultScore.ToString();
        comboText.text = "MaxCombo" + resultCombo.ToString();
        totalText.text = "TotalScore\n" + TotalScore.ToString();
    }

    
    void Update()
    {
        
    }
    public void ShowResult()
    {
        //GameObject resultObj = Instantiate(resultPrefab, new Vector3(0,0,-5),Quaternion.identity);
        GameObject resultObj = Instantiate(resultPrefab);

        TriangleMesh.instance.resultPanel = resultObj.transform.Find("Panel").GetComponent<SpriteRenderer>();
        triangle.SetActive(true);
    }
}
