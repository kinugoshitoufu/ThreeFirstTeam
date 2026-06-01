using TMPro;
using UnityEngine;
using System.Collections;
public class ResultScript : MonoBehaviour
{
    public static ResultScript instance;
    private Animator animator;
    [Header("テキスト")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI totalText;
    public int TotalScore = 0;
    public static float resultTime;    //リザルト用
    public static int resultScore;     //リザルト用
    public static int resultCombo;        //リザルト用
    [Header("プレハブ入れてね！")]
    public GameObject resultPrefab;
    public GameObject resultPanel;
    public GameObject triangle;
    public Transform player;
    public bool isFalling = false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        triangle.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = resultPanel.GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }
    
    public void ShowResult()
    {
        GameObject resultObj = Instantiate(resultPrefab);

        timeText = resultObj.transform.Find("Canvas/Time").GetComponent<TextMeshProUGUI>();
        scoreText = resultObj.transform.Find("Canvas/Score").GetComponent<TextMeshProUGUI>();
        comboText = resultObj.transform.Find("Canvas/Combo").GetComponent<TextMeshProUGUI>();
        totalText = resultObj.transform.Find("Canvas/TotalScore").GetComponent<TextMeshProUGUI>();

        int TotalScore =
        (int)resultTime + resultScore + resultCombo;

        timeText.text = "Life Time:" + resultTime.ToString("F0");

        scoreText.text = "Score:" + resultScore.ToString();

        comboText.text = "MaxCombo:" + resultCombo.ToString();

        totalText.text = "TotalScore" + TotalScore.ToString();

        
        //TriangleMesh.instance.resultPanel = resultObj.transform.Find("Panel").GetComponent<SpriteRenderer>();
        triangle.SetActive(true);
        StartCoroutine(ResultCoroutine());
    }
    IEnumerator ResultCoroutine()
    {
        resultPanel.SetActive(true);

        animator.Play("ResultAnim");

        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 0f;
    }
    public static void ResetText()
    {
        resultTime = 0f;
        resultScore = 0;
        resultCombo = 0;
    }
}
