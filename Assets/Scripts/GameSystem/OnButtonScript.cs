using UnityEngine;
using UnityEngine.SceneManagement;

public class OnButtonScript : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void ClickTitleScene()
    {
        SoundManager.Instance.GAMESE(0);
        SceneChanger.instance.sceneChanger("TitleScene");
    }
    public void ClickRetry()
    {
        SoundManager.Instance.GAMESE(0);
        SceneChanger.instance.sceneChanger("Wada_Scene");//名前を変えればいいよ！！！
    }
}
