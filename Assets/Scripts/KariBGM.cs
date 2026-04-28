using UnityEngine;

public class KariBGM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            SoundManager.Instance.GAMEBGM(1); // 0番のBGM再生
    }
}
