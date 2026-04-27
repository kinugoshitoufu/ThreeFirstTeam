using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f; // ← これが個別スライダー
    }

    [Header("BGM")]
    public AudioSource bgmSource;
    public SoundData[] bgmList;

    [Header("SE")]
    public AudioSource[] seSources;
    public SoundData[] seList;

    [Header("音量設定")]
    [Range(0f, 1f)]
    public float masterBGMVolume = 1f;

    [Range(0f, 1f)]
    public float masterSEVolume = 1f;

    private Dictionary<string, SoundData> bgmDict;
    private Dictionary<string, SoundData> seDict;

    private string currentBGM = "";
    private Coroutine bgmCoroutine;

    private int seIndex = 0;

    // -------------------------------
    // 初期化
    // -------------------------------
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
        bgmDict = new Dictionary<string, SoundData>();
        seDict = new Dictionary<string, SoundData>();

        foreach (var bgm in bgmList)
        {
            bgmDict[bgm.name] = bgm;
        }

        foreach (var se in seList)
        {
            seDict[se.name] = se;
        }
    }

    // -------------------------------
    // BGM再生（即時）
    // -------------------------------
    public void PlayBGM(string name)
    {
        if (!bgmDict.ContainsKey(name))
        {
            Debug.LogWarning("BGMがない: " + name);
            return;
        }

        if (currentBGM == name) return;

        var data = bgmDict[name];

        bgmSource.clip = data.clip;
        bgmSource.loop = true;
        bgmSource.volume = data.volume * masterBGMVolume;
        bgmSource.Play();

        currentBGM = name;
    }

    // -------------------------------
    // BGM再生（フェード）
    // -------------------------------
    public void PlayBGMWithFade(string name, float fadeTime = 1f)
    {
        if (!bgmDict.ContainsKey(name))
        {
            Debug.LogWarning("BGMがない: " + name);
            return;
        }

        if (currentBGM == name) return;

        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
        }

        bgmCoroutine = StartCoroutine(FadeBGM(name, fadeTime));
    }

    IEnumerator FadeBGM(string name, float fadeTime)
    {
        float startVolume = bgmSource.volume;

        // フェードアウト
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        bgmSource.Stop();

        var data = bgmDict[name];

        // 新しいBGM
        bgmSource.clip = data.clip;
        bgmSource.loop = true;
        bgmSource.volume = 0;
        bgmSource.Play();

        currentBGM = name;

        float targetVolume = data.volume * masterBGMVolume;

        // フェードイン
        while (bgmSource.volume < targetVolume)
        {
            bgmSource.volume += targetVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    // -------------------------------
    // BGM停止
    // -------------------------------
    public void StopBGM()
    {
        bgmSource.Stop();
        currentBGM = "";
    }

    // -------------------------------
    // SE再生（複数同時）
    // -------------------------------
    public void PlaySE(string name)
    {
        if (!seDict.ContainsKey(name))
        {
            Debug.LogWarning("SEがない: " + name);
            return;
        }

        var data = seDict[name];

        AudioSource source = seSources[seIndex];

        float finalVolume = data.volume * masterSEVolume;

        source.PlayOneShot(data.clip, finalVolume);

        seIndex = (seIndex + 1) % seSources.Length;
    }

    // -------------------------------
    // 音量変更
    // -------------------------------
    public void SetBGMVolume(float volume)
    {
        masterBGMVolume = volume;

        if (!string.IsNullOrEmpty(currentBGM) && bgmDict.ContainsKey(currentBGM))
        {
            var data = bgmDict[currentBGM];
            bgmSource.volume = data.volume * masterBGMVolume;
        }
    }

    public void SetSEVolume(float volume)
    {
        masterSEVolume = volume;
    }
}

