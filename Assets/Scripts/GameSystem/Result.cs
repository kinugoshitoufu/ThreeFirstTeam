using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    void Start()
    {
        timeText.text = TimeScript.resultTime.ToString("F0") + "sec";
    }

    
    void Update()
    {
        
    }
}
