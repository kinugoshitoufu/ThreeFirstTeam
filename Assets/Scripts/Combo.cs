using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.BoolParameter;

public class Combo : MonoBehaviour
{
    public TextMeshProUGUI comboTextPrefab;
    public TextMeshProUGUI enemyTextPrefab;
    public RectTransform spawnPoint; // スポーン位置
    public RectTransform centerPoint; // 停止位置
    public PlayerScript playerscript;
    public GameObject comboUI;
    public Vector2 enemyPos;
    public Canvas targetCanvas;
    public Image[] ComboAura;
    private int comboThreshold = 0;
    public float auratimer = 2f;
    private Coroutine auraCoroutine;
    private bool PlayerShotFlag;

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
    private int oldTensDigit = 0;

    private bool isPlaying;

    [Header("表示時間")]
    public float displayTime = 1f;

    [Header("フェードイン速度")]
    public float fadeInSpeed = 2f;

    [Header("フェードアウト速度")]
    public float fadeOutSpeed = 2f;


    // 画面上にいるテキスト
    private TextMeshProUGUI currentLeftText;

    void Start()
    {
        oldcombo = 0;
        comboUI.SetActive(false);
        for (int i = 0; i < ComboAura.Length; i++)
        {
            ComboAura[i].enabled = false;
        }
    }

    void Update()
    {
        ComboCount();
        RankAura();
    }

    public void SetEnemyPos(Vector2 pos)
    {
        enemyPos = pos;
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
            oldTensDigit = 0;
            return;
        }

        if (combo > oldcombo && !isPlaying)
        {
            comboUI.SetActive(true);
            StartCoroutine(PlayComboSequence());
        }
        int currentTensDigit = combo / 10;

        // 2の位が増えた
        if (currentTensDigit > oldTensDigit)
        {
            oldTensDigit = currentTensDigit;

            comboThreshold++;

            int result = comboThreshold * 10;

            Debug.Log(result);

            SpawnEnemyText(enemyPos, result);
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

    void SpawnEnemyText(Vector2 worldPos, int comboValue)
    {
        // ワールド座標 → スクリーン座標
        Vector2 screenPos =Camera.main.WorldToScreenPoint(worldPos);

        // UI生成
        TextMeshProUGUI text = Instantiate(
            enemyTextPrefab,
            targetCanvas.transform
        );

        RectTransform rect = text.rectTransform;

        RectTransform canvasRect = targetCanvas.transform as RectTransform;

        // スクリーン座標 → Canvas座標
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            targetCanvas.worldCamera,
            out Vector2 localPos
        );

        // 初期位置（少し下）
        Vector2 startPos = localPos + Vector2.down * 50f;

        rect.localPosition = startPos;

        // 初期透明度0
        Color color = text.color;
        color.a = 0f;
        text.color = color;

        text.text = comboValue.ToString();

        StartCoroutine(EnemyTextAnimation(text, localPos));
    }

    IEnumerator EnemyTextAnimation(TextMeshProUGUI text, Vector2 targetPos)
    {
        RectTransform rect = text.rectTransform;

        float appearTime = 0.25f;
        float stayTime = 0.5f;
        float disappearTime = 0.4f;

        // ===== 出現 =====
        float timer = 0f;

        Vector2 startPos = targetPos + Vector2.down * 50f;

        while (timer < appearTime)
        {
            timer += Time.deltaTime;

            float t = timer / appearTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // 下→上
            rect.localPosition =
                Vector2.Lerp(startPos, targetPos, t);

            // フェードイン
            Color color = text.color;
            color.a = Mathf.Lerp(0f, 1f, t);
            text.color = color;

            yield return null;
        }

        // 少し待機
        yield return new WaitForSeconds(stayTime);

        // ===== 消える =====
        timer = 0f;

        Vector2 endPos = targetPos + Vector2.up * 50f;

        Color startColor = text.color;

        while (timer < disappearTime)
        {
            timer += Time.deltaTime;

            float t = timer / disappearTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // フェードアウト
            Color color = startColor;
            color.a = Mathf.Lerp(1f, 0f, t);
            text.color = color;

            yield return null;
        }

        Destroy(text.gameObject);
    }

    // 左へ移動
    IEnumerator MoveToLeft(TextMeshProUGUI text)
    {
        RectTransform rect = text.rectTransform;

        Vector2 startPos = spawnPoint.anchoredPosition;
        Vector2 targetPos = centerPoint.anchoredPosition;

        float timer = 0f;

        while (timer < moveTime)
        {
            timer += Time.deltaTime;

            float t = timer / moveTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // 移動
            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

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

    private void RankAura()
    {
        PlayerShotFlag=playerscript.GetShotFlag();
        // E
        //プレイヤーが突撃状態かつ敵が死んだら
        if (combo <= 10 && PlayerShotFlag&&Enemy.isdead)
        {
            ShowAura(4);
        }
        // D
        else if (combo <= 20 && PlayerShotFlag && Enemy.isdead)
        {
            ShowAura(3);
        }
        // C
        else if (combo <= 30 && PlayerShotFlag && Enemy.isdead)
        {
            ShowAura(2);
        }
        // B
        else if (combo <= 40 && PlayerShotFlag && Enemy.isdead)
        {
            ShowAura(1);
        }
        // A
        else if (combo <= 50 && PlayerShotFlag && Enemy.isdead)
        {
            ShowAura(0);
        }
    }

    void ShowAura(int index)
    {
        Enemy.isdead = false;

        // 全部消す
        for (int i = 0; i < ComboAura.Length; i++)
        {
            ComboAura[i].enabled = false;
        }

        // 対象だけ表示
        ComboAura[index].enabled = true;

        // Aura用Coroutineだけ止める
        if (auraCoroutine != null)
        {
            StopCoroutine(auraCoroutine);
        }

        auraCoroutine =
            StartCoroutine(FadeAura(ComboAura[index]));
    }

    IEnumerator FadeAura(Image aura)
    {
        Color color = aura.color;

        // 最初は透明
        color.a = 0f;
        aura.color = color;

        // フェードイン
        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeInSpeed;
            aura.color = color;

            yield return null;
        }

        // 表示維持
        yield return new WaitForSeconds(displayTime);

        // フェードアウト
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime * fadeOutSpeed;
            aura.color = color;

            yield return null;
        }

        aura.enabled = false;
    }
}