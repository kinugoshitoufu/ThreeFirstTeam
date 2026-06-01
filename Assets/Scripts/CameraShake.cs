using System.Collections;
using UnityEngine;

/// <summary>
/// カメラシェイク管理クラス
/// HitStopManager と併用可能。Time.unscaledDeltaTime で動作するため
/// timeScale = 0 中でもシェイクが継続します。
/// </summary>
public class CameraShake : MonoBehaviour
{
    // シングルトン
    public static CameraShake Instance { get; private set; }

    [Header("デフォルト設定")]
    [Tooltip("シェイクの基本強度")]
    [SerializeField] private float defaultIntensity = 0.3f;

    [Tooltip("シェイクのデフォルトフレーム数")]
    [SerializeField] private int defaultFrames = 12;

    [Tooltip("シェイクの減衰カーブ (横軸=進行率 0→1, 縦軸=強度倍率)")]
    [SerializeField] private AnimationCurve dampingCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    // 内部状態
    private Vector3 _originalLocalPos;
    private Coroutine _shakeCoroutine;

    // 現在のシェイク情報（デバッグ用に公開）
    public int RemainFrames { get; private set; } = 0;
    public bool IsShaking => RemainFrames > 0;

    // =========================================================
    // Unity ライフサイクル
    // =========================================================

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _originalLocalPos = transform.localPosition;
    }

    // =========================================================
    // 公開API
    // =========================================================

    /// <summary>
    /// カメラシェイクを開始する（フレーム数指定）
    /// </summary>
    /// <param name="frames">シェイクするフレーム数</param>
    /// <param name="intensity">シェイク強度（省略時はデフォルト値）</param>
    public void Shake(int frames, float intensity = -1f)
    {
        if (frames <= 0) return;

        float resolvedIntensity = intensity < 0f ? defaultIntensity : intensity;

        // 既存シェイクより長いときだけ上書き
        if (frames <= RemainFrames) return;

        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        _shakeCoroutine = StartCoroutine(ShakeRoutine(frames, resolvedIntensity));
    }

    /// <summary>
    /// デフォルト設定でシェイクを開始する
    /// </summary>
    public void ShakeDefault()
    {
        Shake(defaultFrames, defaultIntensity);
    }

    /// <summary>
    /// シェイクを強制停止してカメラ位置をリセットする
    /// </summary>
    public void StopShake()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
            _shakeCoroutine = null;
        }

        RemainFrames = 0;
        transform.localPosition = _originalLocalPos;
    }

    // =========================================================
    // 内部実装
    // =========================================================

    private IEnumerator ShakeRoutine(int totalFrames, float intensity)
    {
        RemainFrames = totalFrames;

        while (RemainFrames > 0)
        {
            // 進行率 (0→1) を計算し、減衰カーブで強度を求める
            float progress = 1f - (float)RemainFrames / totalFrames;
            float currentIntensity = intensity * dampingCurve.Evaluate(progress);

            // ランダムなオフセットを適用
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f) * currentIntensity,
                Random.Range(-1f, 1f) * currentIntensity,
                0f
            );
            transform.localPosition = _originalLocalPos + offset;

            RemainFrames--;

            // unscaledTime で待機 → timeScale=0 のHitStop中も動く
            yield return new WaitForEndOfFrame();
        }

        // シェイク終了後にカメラ位置をリセット
        transform.localPosition = _originalLocalPos;
        RemainFrames = 0;
        _shakeCoroutine = null;
    }

    // =========================================================
    // ギズモ（エディタ上でのデバッグ表示）
    // =========================================================

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!IsShaking) return;
        Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, defaultIntensity);
    }
#endif
}
