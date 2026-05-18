using UnityEngine;

public class TempEnemyScript : MonoBehaviour
{
    public GameObject Enemy;
    
    void spawnEnemy()
    {
        Instantiate(Enemy,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
