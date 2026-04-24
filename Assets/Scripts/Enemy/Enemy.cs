using UnityEngine;

public class Enemy : MonoBehaviour
{
    public System.Action<Enemy> OnDeath;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        // 死亡処理
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
