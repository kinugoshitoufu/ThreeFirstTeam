using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartEnemy : MonoBehaviour
{
    // 川本こうせいが追加したコード↓↓↓↓↓
    [Header("最初の敵の関数達")]
    public static StartEnemy instance;
    public Image tutorialarrow;
    public float targetX = 5f;  //目的地
    public float speed = 3f;    //移動速度
    public Image button;
    private bool ismove = true;
    public static bool Spawnflag = false;
    public bool isdead;

    public GameObject deadEffect;
    public GameObject gameStartEffect;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        FastEnemy();
    }
    void FastEnemy()
    {
        if (ismove)
        {
            tutorialarrow.enabled = false;
            button.enabled = false;
            transform.position += Vector3.left * 3f * Time.deltaTime;
        }
        TimeScript.stopTimer = true;
        // 目的地まで移動
        if (transform.position.x < targetX)
        {
            tutorialarrow.enabled = true;
            FadeManager.instance.SetFadeFlag(false);
            button.enabled = true;
            ismove = false;
        }
    }

    public void FastEnemyDead()
    {

        //Effect
        Instantiate(gameStartEffect,Vector3.zero, Quaternion.identity);
        Instantiate(deadEffect,transform.position, Quaternion.identity);
        
        isdead = true;
        tutorialarrow.enabled = false;
        button.enabled = false;
        TimeScript.stopTimer = false;
        EnemySpawner1.IsMove(true);
        EnemySpawner2.IsMove(true);
        EnemySpawner3.IsMove(true);
        return;
    }
}
