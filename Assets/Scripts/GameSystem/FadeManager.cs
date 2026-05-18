using UnityEngine;
using UnityEngine.UI;
public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
    [Header("フェード用設定")]
    public Image fadeImage;
    public SpriteRenderer tutorialBackScreen;
    public GameObject tutorialBackScreenObject;
    public Transform player;
    public float fadeSpeed = 0.5f;
    private float targetAlpha = 0.0f;
    private bool FadeFlag = true;

    private void Awake()
    {
        instance = this;
        tutorialBackScreenObject.SetActive(false);
    }

    void Update()
    {
        Color color = fadeImage.color;
        if (FadeFlag)
        {
            if (tutorialBackScreenObject.activeSelf)
            {
                tutorialBackScreenObject.SetActive(false);
                fadeImage.color = tutorialBackScreen.color;
            }
            color.a = Mathf.MoveTowards(
                color.a,
                targetAlpha,
                fadeSpeed * Time.deltaTime
            );

            fadeImage.color = color;
        }
        if (!FadeFlag)
        {
            if (tutorialBackScreen != null && tutorialBackScreenObject != null)
            {
                if (!tutorialBackScreenObject.activeSelf)
                {
                    tutorialBackScreenObject.SetActive(true);
                    tutorialBackScreen.color = fadeImage.color;
                    fadeImage.color = new Color(0, 0, 0, 0);
                }
            }
        }

    }

    public void StartFade()
    {
        targetAlpha = 0.0f;
    }

    public bool GetFadeFlag()
    {
        return FadeFlag;
    }

    public void SetFadeFlag(bool f)
    {
        FadeFlag = f;
    }
}
