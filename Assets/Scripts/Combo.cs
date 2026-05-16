using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    public TextMeshProUGUI evencombotext;   // even=偶数
    public TextMeshProUGUI oddcombotext;  // odd=奇数

    public PlayerScript playerscript;

    private int combo;      //現時点のコンボ
    private int oldcombo;   //検知用のコンボ

    private Vector2 evenposx;
    private Vector2 oddposx;

    void Start()
    {
        // ワールド座標
        evenposx = evencombotext.rectTransform.anchoredPosition;
        oddposx = oddcombotext.rectTransform.anchoredPosition;

        // 初期表示
        evencombotext.text = "0";
        oddcombotext.text = "0";

        oldcombo = 0;
    }
    void Update()
    {
        ComboCount();
    }

    void ComboCount()
    {
        //コンボ取得
        combo = playerscript.GetMaxComboCount();

        // コンボが増えたら
        if (combo > oldcombo)
        {
            Vector2 temp = evencombotext.rectTransform.anchoredPosition;

            evencombotext.rectTransform.anchoredPosition =
                oddcombotext.rectTransform.anchoredPosition;

            oddcombotext.rectTransform.anchoredPosition = temp;

            // 偶数
            if (combo % 2 == 0)
            {
                evencombotext.text = combo.ToString();
            }
            // 奇数
            else
            {
                oddcombotext.text = combo.ToString();
            }

            // 前回値更新
            oldcombo = combo;
        }
    }
}