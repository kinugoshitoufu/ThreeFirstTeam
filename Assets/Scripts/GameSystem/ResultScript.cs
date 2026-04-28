using TMPro;
using UnityEngine;

public class ResultScript : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    void Start()
    {
        timeText.text = TimeScript.resultTime.ToString("F0") + "sec";
        scoreText.text = TimeScript.resultScore.ToString() + "point";
        Debug.Log("リザルトスコア："+ TimeScript.resultScore);
    }

    
    void Update()
    {
        
    }
}
