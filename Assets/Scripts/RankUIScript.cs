using UnityEngine;
using UnityEngine.UI;

public class RankUIScript : MonoBehaviour
{
    public Image Rankimage;
    public Sprite[] numbers;
    public GameObject comboUI;
    public GameObject rankUI;
    void Start()
    {   
        comboUI.SetActive(false);
        rankUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.instance.GetComboCount() > 9)
        {
            comboUI.SetActive(true);
            rankUI.SetActive(true);
            if (PlayerScript.instance.GetRank() == "A")
            {
                Rankimage.sprite = numbers[4];
            }
            if (PlayerScript.instance.GetRank() == "B")
            {
                Rankimage.sprite = numbers[3];
            }
            if (PlayerScript.instance.GetRank() == "C")
            {
                Rankimage.sprite = numbers[2];
            }
            if (PlayerScript.instance.GetRank() == "D")
            {
                Rankimage.sprite = numbers[1];
            }
            if (PlayerScript.instance.GetRank() == "E")
            {
                Rankimage.sprite = numbers[0];
            }
        }
        else
        {
            comboUI.SetActive(false);
            rankUI.SetActive(false);
        }
    }
}
