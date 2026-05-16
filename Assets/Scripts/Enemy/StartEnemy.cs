using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartEnemy : MonoBehaviour
{
    // 川本こうせいが追加したコード↓↓↓↓↓
    [Header("最初の敵の関数達")]
    public Image tutorialarrow;
    public float targetX = 5f;  //目的地
    public float speed = 3f;    //移動速度
    public Image backscreen; //黒背景
    public Image button;
    private bool ismove = true;
    public static bool Spawnflag = false;
    public bool isdead;

    public GameObject gameStartEffect;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 黒背景の初期透明度
        Color color = backscreen.color;
        color.a = 0f;
        backscreen.color = color;
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
            button.enabled = true;
            ismove = false;
            // 到着したら透明度0.5
            Color color = backscreen.color;
            color.a = 0.5f;
            backscreen.color = color;
            
        }
    }

    public void FastEnemyDead()
    {
        isdead = true;
        tutorialarrow.enabled = false;
        button.enabled = false;
        TimeScript.stopTimer = false;
        Color color = backscreen.color;
        color.a = 0f;
        backscreen.color = color;
        EnemySpawner1.IsMove(true);

        //Effect
        Instantiate(gameStartEffect,Vector3.zero, Quaternion.identity);
        return;
    }
}
