using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource[] seSources; 
    public AudioClip[] seClips;

    void Awake()
    {
        Instance = this;
    }

    public void GAMESE(int index)
    {
        if (index < 0 || index >= seClips.Length) return;

        // 空いてるAudioSourceを探す
        foreach (var source in seSources)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(seClips[index]);
                return;
            }
        }
    }
}
