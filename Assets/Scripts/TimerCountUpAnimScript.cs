using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerCountUpAnimScript : MonoBehaviour
{
    public static TimerCountUpAnimScript Instance;
    [Header("移動先の設定")]
    [Tooltip("テキストが出発する基準RectTransform")]
    public RectTransform startRect;

    [Tooltip("テキストが向かう終了RectTransform")]
    public RectTransform endRect;

    [Header("アニメーション設定")]
    [Tooltip("アニメーション全体の時間（秒）")]
    public float duration = 1.0f;

    [Tooltip("フェードアウトが始まるタイミング（0〜1 の割合）")]
    [Range(0f, 1f)]
    public float fadeStartRatio = 0.3f;

    [Tooltip("移動のイージング曲線")]
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("テキスト設定")]
    [Tooltip("フォントサイズ")]
    public float fontSize = 28f;

    [Tooltip("テキストの色")]
    public Color textColor = Color.white;

    [Header("プレハブ参照（オプション）")]
    [Tooltip("TextMeshProUGUI プレハブ。未設定の場合は自動生成します")]
    public TextMeshProUGUI textPrefab;

    // アニメーション中に生成したテキストの親となるCanvas
    private Canvas _rootCanvas;

    void Start()
    {
        Instance = this;
    }

    private void Awake()
    {
        // ルートCanvasを取得（スクリーン座標変換に使用）
        _rootCanvas = GetComponentInParent<Canvas>();
        if (_rootCanvas == null)
            _rootCanvas = FindFirstObjectByType<Canvas>();
    }

    /// <summary>
    /// アニメーションを開始します。
    /// </summary>
    /// <param name="message">表示するテキスト</param>
    public void PlayAnimation(string message)
    {
        StartCoroutine(AnimateText(message));
    }

    private IEnumerator AnimateText(string message)
    {
        // --- テキストオブジェクトを生成 ---
        TextMeshProUGUI textObj = CreateTextObject(message);

        RectTransform rt = textObj.rectTransform;

        // 開始・終了位置をスクリーン座標経由でテキストのローカル座標に変換
        // ※ Screen Space - Overlay では worldCamera が null になるため
        //    RectTransformUtility を使わず screenPosition を直接利用する
        Vector2 startPos = ScreenToRectLocal(rt, startRect.position);
        Vector2 endPos = ScreenToRectLocal(rt, endRect.position);

        rt.position = startRect.position;   // まず startRect のワールド位置に合わせる

        float elapsed = 0f;
        CanvasGroup cg = textObj.GetComponent<CanvasGroup>();

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 移動（イージングあり）
            float easedT = moveCurve.Evaluate(t);
            rt.position = Vector3.Lerp(startRect.position, endRect.position, easedT);

            // フェードアウト
            if (t >= fadeStartRatio)
            {
                float fadeT = (t - fadeStartRatio) / (1f - fadeStartRatio);
                cg.alpha = Mathf.Lerp(1f, 0f, fadeT);
            }

            yield return null;
        }

        Destroy(textObj.gameObject);
    }

    /// <summary>
    /// テキストオブジェクトをCanvas直下に生成します。
    /// プレハブのアンカー・ピボット設定はそのまま維持し、
    /// position（ワールド座標）で位置制御するため上書きしません。
    /// </summary>
    private TextMeshProUGUI CreateTextObject(string message)
    {
        TextMeshProUGUI textObj;

        if (textPrefab != null)
        {
            textObj = Instantiate(textPrefab, _rootCanvas.transform);
        }
        else
        {
            // プレハブなし → 自動生成（アンカー中央・ピボット中央）
            var go = new GameObject("FloatingText", typeof(RectTransform), typeof(CanvasGroup));
            go.transform.SetParent(_rootCanvas.transform, false);

            textObj = go.AddComponent<TextMeshProUGUI>();
            textObj.fontSize = fontSize;
            textObj.color = textColor;
            textObj.alignment = TextAlignmentOptions.Center;

            RectTransform rt = textObj.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
        }

        textObj.text = message;

        // CanvasGroup がなければ追加（フェード用）
        if (textObj.GetComponent<CanvasGroup>() == null)
            textObj.gameObject.AddComponent<CanvasGroup>();

        return textObj;
    }

    /// <summary>
    /// ワールド座標をスクリーン座標に変換するユーティリティ（参考用）。
    /// Overlay モードでは worldCamera = null でも動作します。
    /// </summary>
    private Vector2 ScreenToRectLocal(RectTransform rt, Vector3 worldPos)
    {
        // Overlay は worldCamera=null、Camera/World は該当カメラを渡す
        Camera cam = _rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : _rootCanvas.worldCamera;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt.parent as RectTransform, screenPoint, cam, out Vector2 local);
        return local;
    }
}
