using UnityEngine;

public class ShotGageScript : MonoBehaviour
{
    public static ShotGageScript instance;
    public GameObject GageObj;
    public RectTransform GageTransform;
    public float StartY = 0f;
    public float EndY = -2201f;
    void Start()
    {
        instance = this;
        GageObj.SetActive(false);

    }

    void Update()
    {
        if (PlayerScript.instance.GetShotFlag() == true)
        {
            GageObj.SetActive(true);
            float t = PlayerScript.instance.GetShotTimeCount() / PlayerScript.instance.GetShotTime();
            float y = Mathf.Lerp(StartY, EndY, t);
            GageTransform.anchoredPosition = new Vector2(GageTransform.anchoredPosition.x, y);
        }
        else
        {
            GageObj.SetActive(false);
            GageTransform.anchoredPosition = new Vector2(GageTransform.anchoredPosition.x, StartY);
        }
    }
}
