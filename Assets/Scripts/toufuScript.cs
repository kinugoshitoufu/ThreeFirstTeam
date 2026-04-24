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
        text.text = PlayerScript.instance.HoriVert().ToString() + "  " + PlayerScript.instance.shotCount.ToString();
    }
}
