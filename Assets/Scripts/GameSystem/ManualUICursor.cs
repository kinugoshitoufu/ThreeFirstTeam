using UnityEngine;
using UnityEngine.UI;

public class ManualUICursor : MonoBehaviour
{
    public RectTransform cursor;
    public RectTransform[] targets;

    [Header("入力設定")]
    public float stickThreshold = 0.5f;   // スティック反応感度
    public float inputDelay = 0.2f;      // 連続入力防止

    private int index = 0;
    private float timer = 0f;

    void Start()
    {
        MoveCursor();
    }

    void Update()
    {
        if (targets == null || targets.Length == 0) return;

        timer += Time.unscaledDeltaTime;

        float vertical = 0f;

        // ■ キーボード
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            vertical = -1;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            vertical = 1;

        // ■ コントローラー
        vertical += Input.GetAxis("Vertical");

        // ■ 移動処理（連続暴発防止）
        if (Mathf.Abs(vertical) > stickThreshold && timer >= inputDelay)
        {
            if (vertical > 0)
            {
                index--;
                if (index < 0) index = targets.Length - 1;
            }
            else if (vertical < 0)
            {
                index++;
                if (index >= targets.Length) index = 0;
            }

            MoveCursor();
            timer = 0f;
        }

        // ■ 決定ボタン（Enter / Z / コントローラーA）
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            Execute();
        }
    }

    void MoveCursor()
    {
        if (cursor == null) return;

        cursor.position = targets[index].position;
    }

    void Execute()
    {
        Button btn = targets[index].GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.Invoke();
        }
    }
}