using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    private string SceneName;
    public static SceneChanger instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        //SpacesceneChange();
    }
    public void SpacesceneChange()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
    public void sceneChanger(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        if(SceneName == null)
        {
            Debug.Log("SceneName に何も入ってません");
        }
    }
    
}
