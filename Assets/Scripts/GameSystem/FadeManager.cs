using UnityEngine;
using UnityEngine.UI;
public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
    [Header("フェード用設定")]
    public Image fadeImage;

    public Transform player;
    public float fadeSpeed = 0.5f;
    private float startY = 0.0f;
    private float groundY = 0.0f;
    private float totalDistance = 0.0f;
    private float targetAlpha = 0.0f;
    private bool isFade = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        Color color = fadeImage.color;

        color.a = Mathf.MoveTowards(
            color.a,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
        fadeImage.color = color;

    }

    public void StartFade()
    {
        targetAlpha = 0.4f;
    }
}
