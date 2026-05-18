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
    public void ClickRetry(string s)
    {
        SoundManager.Instance.GAMESE(0);
        SceneChanger.instance.sceneChanger(s);//名前を変えればいいよ！！！
    }
}
