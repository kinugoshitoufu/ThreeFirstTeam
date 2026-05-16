using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
    public AudioClip[] seClips;
    public static SoundManager2 instance;

    private AudioSource[] seAudios;

    public int maxSeAudio = 10;

    private int currentAudioIndex = 0;

    void Start()
    {
        instance = this;

        seAudios = new AudioSource[maxSeAudio];

        for (int i = 0; i < maxSeAudio; i++)
        {
            seAudios[i] = gameObject.AddComponent<AudioSource>();
            seAudios[i].loop = false;
            seAudios[i].playOnAwake = false;
            seAudios[i].spatialBlend = 0f;
        }
    }

    public void PlaySE(int index)
    {
        if (index < 0 || index >= seClips.Length) return;

        seAudios[currentAudioIndex].PlayOneShot(seClips[index]);

        currentAudioIndex++;

        if (currentAudioIndex >= maxSeAudio)
        {
            currentAudioIndex = 0;
        }
    }
}
