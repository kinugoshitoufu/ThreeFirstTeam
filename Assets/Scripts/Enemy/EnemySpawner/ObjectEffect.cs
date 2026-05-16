using UnityEngine;

public class ObjectEffect : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        GameObject.Destroy(gameObject);
        //アニメーション終了時の処理
    }
}
