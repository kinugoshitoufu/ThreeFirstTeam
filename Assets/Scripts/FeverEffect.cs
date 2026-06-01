using UnityEngine;

public class FeverEffect : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        if (FeverManeger.IsFever)
        {
            Instantiate(gameObject,transform.position,Quaternion.identity);
        }
        GameObject.Destroy(gameObject);
        //アニメーション終了時の処理
    }
}
