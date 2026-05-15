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
        //どこでも使えるように
        Instance = this;
    }

    private void Update()
    {

    }

    public void GAMESE(int index)
    {
        if (index < 0 || index >= seList.Length) return;

        AudioSource source = seSources[0];

        // 同じ音なら最初から再生
        if (source.clip == seList[index].clip && source.isPlaying)
        {
            source.time = 0f;
            return;
        }

        // 別音なら再生
        source.clip = seList[index].clip;
        source.volume = seList[index].SEvolume;
        source.Play();
    }

    public void GAMEBGM(int index)
    {
        if (index < 0 || index >= bgmList.Length) return;

        foreach (var source in bgmSource)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(bgmList[index].clip, bgmList[index].BGMvolume);
                return;
            }
        }
    }
}
