using System.Collections;
using TMPro;
using UnityEngine;

public class Combo : MonoBehaviour
{
    public TextMeshProUGUI comboTextPrefab;

    public RectTransform spawnPoint; // スポーン位置
    public RectTransform leftPoint;  // 停止位置

    public PlayerScript playerscript;

    public GameObject comboUI;

    [Header("移動速度")]
    public float moveTime = 0.25f;

    [Header("初期サイズと最終サイズ")]
    public float startScale = 0.5f;
    public float endScale = 2.0f;

    [Header("透明度")]
    public float startAlpha = 0f;

    [Header("消える速度")]
    public float destroyTime = 0.3f;

    private int combo;
    private int oldcombo;

    private bool isPlaying;

    // 今左にいるテキスト
    private TextMeshProUGUI currentLeftText;

    void Start()
    {
        oldcombo = 0;
        comboUI.SetActive(false);
    }

    void Update()
    {
        ComboCount();
    }

    IEnumerator PlayComboSequence()
    {
        isPlaying = true;

        while (oldcombo < combo)
        {
            oldcombo++;

            SpawnNewText(oldcombo);

            yield return new WaitForSeconds(0.08f);

            // 毎回最新コンボ取得
            combo = playerscript.GetComboCount();
        }

        isPlaying = false;
    }

    void ComboCount()
    {
        combo = playerscript.GetComboCount();

        // コンボ0なら消す
        if (combo <= 0)
        {
            if (currentLeftText != null)
            {
                Destroy(currentLeftText.gameObject);
                currentLeftText = null;
            }
            comboUI.SetActive(false);
            oldcombo = 0;
            return;
        }

        if (combo > oldcombo && !isPlaying)
        {
            comboUI.SetActive(true);
            StartCoroutine(PlayComboSequence());
        }
    }

    void SpawnNewText(int value)
    {
        // 文字を消す
        if (currentLeftText != null)
        {
            StartCoroutine(ExpandAndFadeOut(currentLeftText));
        }

        // 新しい文字生成
        TextMeshProUGUI newText = Instantiate(
            comboTextPrefab,
            spawnPoint.position,
            Quaternion.identity,
            spawnPoint.parent
        );

        RectTransform rect = newText.rectTransform;

        // 初期位置
        rect.anchoredPosition = spawnPoint.anchoredPosition;

        // 初期スケール
        rect.localScale = Vector3.one * startScale;

        // 数値
        newText.text = value.ToString();

        // 初期透明度
        Color color = newText.color;
        color.a = startAlpha;
        newText.color = color;

        // 現在の左テキスト更新
        currentLeftText = newText;

        // 左へ移動
        StartCoroutine(MoveToLeft(newText));
    }

    // 左へ移動
    IEnumerator MoveToLeft(TextMeshProUGUI text)
    {
        RectTransform rect = text.rectTransform;

        Vector2 startPos = spawnPoint.anchoredPosition;
        Vector2 targetPos = leftPoint.anchoredPosition;

        float timer = 0f;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;

            float t = timer / moveTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // 移動
            rect.anchoredPosition =
                Vector2.Lerp(startPos, targetPos, t);

            // スケール
            float scale =
                Mathf.Lerp(startScale, 1f, t);

            rect.localScale =
                Vector3.one * scale;

            // フェード
            Color color = text.color;

            color.a =
                Mathf.Lerp(startAlpha, 1f, t);

            text.color = color;

            yield return null;
        }

        rect.anchoredPosition = targetPos;
    }

    // 拡大してフェードアウトする
    IEnumerator ExpandAndFadeOut(TextMeshProUGUI text)
    {
        RectTransform rect = text.rectTransform;

        float timer = 0f;

        Vector3 start = rect.localScale;
        Vector3 end = Vector3.one * endScale;

        Color startColor = text.color;

        while (timer < destroyTime)
        {
            timer += Time.deltaTime;

            float t = timer / destroyTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // 拡大
            rect.localScale =
                Vector3.Lerp(start, end, t);

            // フェードアウト
            Color color = startColor;

            color.a =
                Mathf.Lerp(1f, 0f, t);

            text.color = color;

            yield return null;
        }

        Destroy(text.gameObject);
    }
}