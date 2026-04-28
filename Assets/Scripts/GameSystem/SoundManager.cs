using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class SEData
    {
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f; // ← インスペクターで調整できる
    }

    public AudioSource[] seSources;
    public SEData[] seList; // ← 変更ポイント

    void Awake()
    {
        Instance = this;
    }

    public void GAMESE(int index)
    {
        if (index < 0 || index >= seList.Length) return;

        foreach (var source in seSources)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(seList[index].clip, seList[index].volume);
                return;
            }
        }
    }
}
