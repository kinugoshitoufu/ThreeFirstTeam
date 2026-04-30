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
        SceneChanger.instance.sceneChanger("TitleScene");
    }
    public void ClickRetry()
    {
        SceneChanger.instance.sceneChanger("Wada_Scene");//名前を変えればいいよ！！！
    }
}
