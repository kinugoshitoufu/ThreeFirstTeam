using UnityEngine;
using UnityEngine.UI;

public class RankUIScript : MonoBehaviour
{
    public Image Rankimage;
    public Sprite[] numbers;
    public GameObject rankUI;
    public RectTransform RankRect;
    public GameObject RankParticle;
    public new UnityEngine.Camera camera;
    private bool ParticleFlag = false;
    void Start()
    {
        rankUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.instance.GetComboCount() >= PlayerScript.instance.combocountRank)
        {
            rankUI.SetActive(true);
            if (PlayerScript.instance.GetRank() == "A")
            {
                Rankimage.sprite = numbers[4];
                if (!ParticleFlag)
                {
                    SpawnRankParticle();
                    ParticleFlag = true;
                }
            }
            if (PlayerScript.instance.GetRank() == "B")
            {
                Rankimage.sprite = numbers[3];
                if (!ParticleFlag)
                {
                    SpawnRankParticle();
                    ParticleFlag = true;
                }
            }
            if (PlayerScript.instance.GetRank() == "C")
            {
                Rankimage.sprite = numbers[2];
                if (!ParticleFlag)
                {
                    SpawnRankParticle();
                    ParticleFlag = true;
                }
            }
            if (PlayerScript.instance.GetRank() == "D")
            {
                Rankimage.sprite = numbers[1];
                if (!ParticleFlag)
                {
                    SpawnRankParticle();
                    ParticleFlag = true;
                }
            }
            if (PlayerScript.instance.GetRank() == "E")
            {
                Rankimage.sprite = numbers[0];
                if (!ParticleFlag)
                {
                    SpawnRankParticle();
                    ParticleFlag = true;
                }
            }
        }
        else
        {
            rankUI.SetActive(false);
            ParticleFlag = false;
        }
    }

    void SpawnRankParticle()
    {
        Vector2 ScreenPos = RectTransformUtility.WorldToScreenPoint(null, RankRect.position);
        Vector3 WorldPos = camera.ScreenToWorldPoint(new Vector3(ScreenPos.x,ScreenPos.y,-10.0f));
        WorldPos.z = 0;
        Instantiate(RankParticle, WorldPos, Quaternion.identity);
    }
}
