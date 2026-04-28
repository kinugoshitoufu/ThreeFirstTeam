using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource[] seSources;
    public SEData[] seList;

    public AudioSource[] bgmSource;
    public BGMData[] bgmList;

    [System.Serializable]
    public class SEData
    {
        public AudioClip clip;

        [Range(0f, 1f)]
        public float SEvolume = 1f;
    }

    [System.Serializable]
    public class BGMData
    {
        public AudioClip clip;

        [Range(0f, 1f)]
        public float BGMvolume = 1f;
    }

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
                source.PlayOneShot(seList[index].clip, seList[index].SEvolume);
                return;
            }
        }
    }

    public void GAMEBGM(int index)
    {
        if (index < 0 || index >= bgmList.Length) return;

        foreach (var source in seSources)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(bgmList[index].clip, bgmList[index].BGMvolume);
                return;
            }
        }
    }
}
