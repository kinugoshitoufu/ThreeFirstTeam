using TMPro;
using UnityEngine;

public class toufuScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = PlayerScript.instance.GetHoriVert().ToString() + "  (" + PlayerScript.instance.shotCount.ToString() + ")  ("
            + PlayerScript.instance.GetComboCount().ToString() + ")  (" + PlayerScript.instance.GetMaxComboCount().ToString() + ")  ("
            + PlayerScript.instance.GetScore().ToString() + ")";
    }
}
