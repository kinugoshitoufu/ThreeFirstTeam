using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
    public AudioClip[] seClips; // SEの音源データ
    public static SoundManager2 instance; // シングルトンの実体
    private AudioSource[] seAudios;// SE用のプレイヤー
    public int maxSeAudio = 10;  // SE用のプレイヤーの最大数
    void Start()
    {
        instance = this;

        seAudios = new AudioSource[maxSeAudio];
        for (int i = 0; i < maxSeAudio; i++)
        {
            seAudios[i] = gameObject.AddComponent<AudioSource>();
            seAudios[i].loop = false; // ループ再生を無効化
            seAudios[i].playOnAwake = false;// 自動再生無効化
        }
    }
    public void PlaySE(int index)
    {
        if (index < 0) return; // indexが0未満なら何もしない
        if(index >= seClips.Length) return;// indexが範囲外なら何もしない
        for (int i = 0; i < maxSeAudio; i++)
        {  // 再生中ではないプレイヤーを探す
            if (seAudios[i].isPlaying) continue;// 再生中なら次へ
            seAudios[i].PlayOneShot(seClips[index]);// SEを再生
            break;// SEを鳴らしたらfor文を抜ける
        }
    }
}
