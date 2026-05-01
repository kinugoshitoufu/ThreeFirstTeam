using System;
using Unity.Mathematics;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerScript : MonoBehaviour
{
　　//パラメーターなど
    public static PlayerScript instance;
    public GameObject Arrow;
    public float movespeed = 3.0f;
    public Vector2 moveArea = new(8.0f, 4.5f);//動ける範囲
    public float Shotspeed = 10.0f;
    public float shottime = 3.0f;
    public float gravity = 1.0f;
    public int shotCount = 1;
    public float combotime = 1.2f;
    public int scorecount = 200;
    public float timeup = 1.0f;
    public int downfallspeed = 30;
    public float fallspeed = 0.5f;
    public TimeScript Timescript;
    public float shotcollider = 0.1f;
    public float knockback = 10.0f;


    //操作によって変更
    private int control = 0;//操作方法の変更

    //フラグ・カウント系など
    [SerializeField] private string rank = "none";
    private Animator animator;
    private Vector3 PlayerPos;
    private BoxCollider2D box;
    private int combocount = 0;
    private bool shotboxflag = true;
    private int score = 0;
    private bool shotFlag;
    public int maxcombo = 0;
    [SerializeField] private float shottimecount = 0.0f;
    [SerializeField] private float combotimecount = 0.0f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        
        box = GetComponent<BoxCollider2D>();
        PlayerPos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody2D>();
        shotFlag = false;
        shotboxflag = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            combocount++;
            combotimecount = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            control = 0;
            Debug.Log("コントローラー操作に変更しました");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            control = 1;
            Debug.Log("キーボード操作に変更しました");
        }
        
        MoveAreaCheck();
        if (!shotFlag)
        {
            Move();
        }
        PlayerPos = transform.position;
        if (shotCount > 0 && !shotFlag && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDamage"))
        {
            if (control == 0 && Input.GetKeyDown("joystick button 2"))
            {
                Debug.Log("コントローラー突撃");
                shot();
            }
            if (control == 1 && Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("キーボード突撃");
                shot();
            }
        }
        if (shotFlag)
        {
            shottimecount += Time.deltaTime;
            rb.gravityScale = 0.0f;
        }
        if (shottime < shottimecount)
        {
            shotFlag = false;
            rb.linearVelocity = Vector3.zero;
            rb.gravityScale = gravity;
            shottimecount = 0.0f;
        }
        if (maxcombo < combocount)
        {
            maxcombo = combocount;
        }
        if (combocount > 0)
        {
            combotimecount += Time.deltaTime;            
        }
        if (combotime < combotimecount)
        {
            gravity = 1.0f;
            rb.gravityScale = gravity;
            combocount = 0;
            combotimecount = 0.0f;
        }
        if (combocount > downfallspeed)
        {
            gravity = fallspeed;
        }
        if (shotboxflag && shotFlag)
        {
            rankbox();
        }
        else
        {
            box.size = Vector2.one;
        }
        comborank();
        if (Input.anyKeyDown)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            { // 検索
                if (Input.GetKeyDown(code))
                { // 入力されたキーの名前と一致した場合
                    Debug.Log(code.ToString() + " のボタンが押されたよ！！"); // コンソールに表示
                }
            }
        }
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDamage"))
        //{
        //    Debug.Log("Damage再生中");
        //}
        //else
        //{
        //    Debug.Log("Damage終了");
        //}
    }

    void Move()
    {
        //スティック入力をVecter2型に代入
        Vector2 vec;
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");
        // ベクトルの長さが１を超えていたら長さを１にする
        

        //移動量を算出する
        vec *= movespeed * Time.deltaTime;
        //実際にプレイヤーを動かす
        //rb.linearVelocityX = vec.x;
        transform.Translate(vec.x, 0.0f, 0.0f);
        // 入力方向を向く角度を計算 (2DなのでZ軸回転)
        float angle = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

        if (Mathf.Abs(vec.x) <= 0.01f)
        {
            animator.SetBool("WalkAnim", false);
        }
        else
        {
            animator.SetBool("WalkAnim", true);
        }
        
        // 現在の回転から、計算した角度へ徐々に回転させる
        Arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);
        //Vector2 arrowangletemp = new Vector2 (vec.x - Arrow.transform.position.x,vec.y - Arrow.transform.position.y);
        //float arrowangle = Mathf.Atan2(arrowangletemp.x, arrowangletemp.y) * Mathf.Rad2Deg - 90f;

    }

    void shot()
    {
        rb.linearVelocity *= 0.0f;
        rb.AddForce(Arrow.transform.up * Shotspeed, ForceMode2D.Impulse);
        shotFlag = true;
        animator.Play("PlayerShot", 0, 0.0f);
        shotCount--;
    }

    void MoveAreaCheck()
    {
        Vector3 pos = transform.position;
        //Vector3 vec = rb.linearVelocity;
        if (pos.x > moveArea.x)
        {
            pos.x = moveArea.x;
            //Debug.Log("どれ");
            //vec.x = -vec.x;
            //vec *= 0.0f;
        }
        if (pos.x < -moveArea.x)
        {
            pos.x = -moveArea.x;
            //Debug.Log("それ");
            //vec.x = -vec.x;
            //vec *= 0.0f;
        }
        if (pos.y > moveArea.y)
        {
            pos.y = moveArea.y;
            //Debug.Log("あれ");
            //vec.y = -vec.y;
            //vec *= 0.0f;
        }
        if (pos.y < -moveArea.y)
        {
            pos.y = -moveArea.y;
            //if (rb.linearVelocityY < 0)
            //{
            //    rb.linearVelocityY = 0.0f;
            //}
            if (shotCount < 1)
            {
                shotCount = 1;
            }
            animator.SetBool("FallAnim", false);
            shotboxflag = false;
            //Debug.Log("これ");
            //vec.y = -vec.y;
            //vec *= 0.0f;
        }
        else
        {
            animator.SetBool("FallAnim", true);
            shotboxflag = true;
        }
        transform.position = pos;
        //rb.linearVelocity = vec;

    }

    void comborank()
    {
        if (combocount > 49)
        {
            rank = "A";
        }
        else if (combocount > 39)
        {
            rank = "B";
        }
        else if (combocount > 29)
        {
            rank = "C";
        }
        else if (combocount > 19)
        {
            rank = "D";
        }
        else if (combocount > 9)
        {
            rank = "E";
        }
        else
        {
            rank = "none";
        }
    }

    void rankbox()
    {
        if (rank == "A")
        {
            box.size = new Vector2(1.0f + (shotcollider * 5), 1.0f + (shotcollider * 5));
        }
        else if (rank == "B")
        {
            box.size = new Vector2(1.0f + (shotcollider * 4), 1.0f + (shotcollider * 4));
        }
        else if (rank == "C")
        {
            box.size = new Vector2(1.0f + (shotcollider * 3), 1.0f + (shotcollider * 3));
        }
        else if (rank == "D")
        {
            box.size = new Vector2(1.0f + (shotcollider * 2), 1.0f + (shotcollider * 2));
        }
        else if (rank == "E")
        {
            box.size = new Vector2(1.0f + (shotcollider * 1), 1.0f + (shotcollider * 1));
        }
        else
        {
            box.size = new Vector2(1.0f, 1.0f);
        }
    }

    public Vector2 GetHoriVert()
    {
        Vector2 vec = new Vector2(0, 0);
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");
        return vec;
    }
    
    public int GetComboCount()
    {
        return combocount;
    }
    public void SetComboCount(int c)
    {
        combocount = c;
    }

    public int GetMaxComboCount()
    {
        return maxcombo;
    }

    public int GetScore()
    {
        return score;
    }
    public void SetScore(int s)
    {
        score = s;
    }
    public string GetRank()
    {
        return rank;
    } 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (shotCount < 1)
            {
                shotCount = 1;
            }
            animator.SetBool("FallAnim",false);
            shotboxflag = false;
            //transform.position = new Vector3(transform.position.x,transform.position.y - (box.size.y - 1.0f));
        }        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            //if (shotCount < 1)
            //{
            //    shotCount = 1;
            //}
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            animator.SetBool("FallAnim", true);
            shotboxflag = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (shotFlag)
            {
                Destroy(collision.gameObject);
                combocount++;
                shotCount++;
                combotimecount = 0.0f;
                score += scorecount * combocount;
                gravity -= fallspeed / downfallspeed;
                Timescript.LimitTime += timeup;
            }
            else
            {
                //rb.AddForce(-Arrow.transform.up * knockback, ForceMode2D.Impulse);
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDamage"))
                {
                    animator.Play("PlayerDamage", 0, 0);
                }
                
            }
        }
        if (collision.CompareTag("EnemyBullet"))
        {
            if (shotFlag)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                //rb.AddForce(-Arrow.transform.up * knockback, ForceMode2D.Impulse);
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDamage"))
                {
                    animator.Play("PlayerDamage", 0, 0);
                }
            }
        }
    }
}
