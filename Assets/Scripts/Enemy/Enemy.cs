using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Action<Enemy> OnDeath;

    public float spawnTime = 0.1f;
    public float speed = 3f; // 敵の移動速度
    private Transform player;
    private Transform enemy;
    public static bool isFlag=true;
    public bool startFlag=false;
    public GameObject deadEffect;
    public Animator enemyDeath1Animator;
    
    void Start()
    {
        Debug.Log("敵生成");
        // プレイヤーのTransformを取得
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && startFlag)
        {
            Die(enemyDeath1Animator, "EnemyDeath1");
        }
    }
    public void Die(Animator animator,string animName)
    {
        Debug.Log("敵消滅");
        //死亡Effect
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        animator.Play(animName);
        OnDeath?.Invoke(this);
        Destroy(gameObject,1.0f);
    }
    private void StartEnemy()
    {
        startFlag = true;
        tag = "Enemy";
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        Debug.Log(startFlag);
    }
    public static void IsMove(bool flag)
    {
        isFlag=flag;
    }
}
