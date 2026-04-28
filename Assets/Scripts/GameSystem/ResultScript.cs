using TMPro;
using UnityEngine;

public class ResultScript : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    void Start()
    {
        timeText.text = TimeScript.resultTime.ToString("F0") + "sec";
        scoreText.text = TimeScript.resultScore.ToString() + "point";
        comboText.text = TimeScript.resultCombo.ToString() + "combo";
    }

    
    void Update()
    {
        
    }
}
