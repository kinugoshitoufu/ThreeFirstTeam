using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
    public AudioClip[] seClips;
    public AudioClip[] bgmClips;
    public static SoundManager2 instance;

    private AudioSource[] seAudios;
    private AudioSource[] bgmAudios;

    [Range(0f,1f)]
    public float bgmVolume;

    public int maxSeAudio = 10;
    public int maxBgmAudio = 3;

    private int currentSeAudioIndex = 0;
    private int currentBgmAudioIndex = 0;

    void Start()
    {
        instance = this;

        seAudios = new AudioSource[maxSeAudio];

        bgmAudios = new AudioSource[maxBgmAudio];

        for (int i = 0; i < maxSeAudio; i++)
        {
            seAudios[i] = gameObject.AddComponent<AudioSource>();
            seAudios[i].loop = false;
            seAudios[i].playOnAwake = false;
            seAudios[i].spatialBlend = 0f;
            seAudios[i].ignoreListenerPause = true;
        }
        for (int i = 0; i < maxBgmAudio; i++)
        {
            bgmAudios[i] = gameObject.AddComponent<AudioSource>();
            bgmAudios[i].volume = bgmVolume;
            bgmAudios[i].loop = true;
            bgmAudios[i].playOnAwake = false;
            bgmAudios[i].spatialBlend = 0f;
            bgmAudios[i].ignoreListenerPause = true;
        }
    }

    public void PlaySE(int index)
    {
        if (index < 0 || index >= seClips.Length) return;

        seAudios[currentSeAudioIndex].PlayOneShot(seClips[index]);

        currentSeAudioIndex++;

        if (currentSeAudioIndex >= maxSeAudio)
        {
            currentSeAudioIndex = 0;
        }
    }

    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Length) return;

        bgmAudios[currentBgmAudioIndex].clip = bgmClips[index];

        bgmAudios[currentBgmAudioIndex].Play();

        currentBgmAudioIndex++;

        if (currentBgmAudioIndex >= maxBgmAudio)
        {
            currentBgmAudioIndex = 0;
        }
    }
}
