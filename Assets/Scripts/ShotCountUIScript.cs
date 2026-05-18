using TMPro;
using UnityEngine;

public class ShotCountUIScript : MonoBehaviour
{
    public TextMeshProUGUI ShotCountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShotCountText.text = PlayerScript.instance.GetShotCount().ToString();
    }
}
