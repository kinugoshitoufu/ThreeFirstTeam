using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Action<Enemy> OnDeath;

    public float spawnTime = 0.1f;
    public float speed = 3f; // 敵の移動速度
    private Transform player;

    public static bool isFlag=true;
    private bool startFlag=false;
    void Start()
    {
        Debug.Log("敵生成");
        // プレイヤーのTransformを取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null&& isFlag&& startFlag)
        {
            // プレイヤーの方向へ移動
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& isFlag&& startFlag)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("敵消滅");
        // 死亡処理
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
    private void StartEnemy()
    {
        startFlag = true;
        tag = "Enemy";
    }
    public static void IsMove(bool flag)
    {
        isFlag=flag;
    }
}
