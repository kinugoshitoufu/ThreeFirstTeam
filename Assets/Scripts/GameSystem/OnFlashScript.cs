using UnityEngine;

public class OnFlashScript : MonoBehaviour
{
    // 点滅させる対象
    [SerializeField] private SpriteRenderer _target;

    // 点滅周期[s]
    [SerializeField] private float _cycle = 1;
    [SerializeField] private float _alpha;

    [SerializeField] private bool _isBlinking;
    [SerializeField] private float _defaultAlpha;
    [SerializeField] private double _time;

    /// <summary>
    /// 点滅を開始する
    /// </summary>
    public void BeginBlink()
    {
        // 点滅中は何もしない
        if (_isBlinking) return;

        _isBlinking = true;

        // 時間を開始時点に戻す
        _time = 0;
    }

    /// <summary>
    /// 点滅を終了する
    /// </summary>
    public void EndBlink()
    {
        _isBlinking = false;

        // 初期状態のアルファ値に戻す
        SetAlpha(_defaultAlpha);
    }

    private void Start()
    {
        _defaultAlpha = _target.color.a;
    }

    private void Update()
    {
        // 点滅していないときは何もしない
        if (!_isBlinking) return;

        // 内部時刻を経過させる
        _time += Time.deltaTime;

        // 周期cycleで繰り返す値の取得
        // 0～cycleの範囲の値が得られる
        var repeatValue = Mathf.Repeat((float)_time, _cycle);

        // 内部時刻timeにおける明滅状態を反映
        // Imageのアルファ値を変更している
        SetAlpha(repeatValue >= _cycle * 0.5f ? 1 : 0);
    }

    // Imageのアルファ値を変更するメソッド
    private void SetAlpha(float alpha)
    {
        var color = _target.color;
        _alpha = alpha;
        color.a = alpha;
        _target.color = color;
    }
}
