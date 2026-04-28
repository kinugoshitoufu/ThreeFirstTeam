using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public string SceneName;
    public static SceneChanger instance;
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    
    void Update()
    {
        
    }
    public void SpacesceneChange()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
    public void sceneChanger()
    {
        SceneManager.LoadScene(SceneName);
    }
    public void TitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void Retry()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Wada_Scene");//名前を変えればいいよ！！！
    }
}
