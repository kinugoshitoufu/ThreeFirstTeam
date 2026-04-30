using TMPro;
using UnityEngine;

public class ResultScript : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    public static float resultTime;    //リザルト用
    public static int resultScore;     //リザルト用
    public static int resultCombo;        //リザルト用
    void Start()
    {
        timeText.text = resultTime.ToString("F0") + "sec";
        scoreText.text = resultScore.ToString() + "point";
        comboText.text = resultCombo.ToString() + "combo";
    }

    
    void Update()
    {
        
    }
}
