using Unity.Mathematics;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
　　//パラメーターなど
    public static PlayerScript instance;
    public GameObject Arrow;
    public float movespeed = 3.0f;
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

    //操作によって変更

    //フラグ・カウント系など
    [SerializeField] private string rank = "none";
    private Vector3 PlayerPos;
    private BoxCollider2D box;
    private int combocount = 0;
    private int score = 0;
    private bool shotFlag;
    private int maxcombo = 0;
    private float shottimecount = 0.0f;
    private float combotimecount = 0.0f;
    private int control = 0;//操作方法の変更
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        box = GetComponent<BoxCollider2D>();
        PlayerPos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody2D>();
        shotFlag = false;
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
        if (!shotFlag)
        {
            Move();
        }
        PlayerPos = transform.position;
        if (shotCount > 0 && !shotFlag && Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("aaaaaajump");
            shot();
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
        comborank();
    }

    void Move()
    {
        //スティック入力をVecter2型に代入
        Vector2 vec = new Vector2(0, 0);
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");
        // ベクトルの長さが１を超えていたら長さを１にする
        //if (vec.sqrMagnitude > 1)
        //{
        //    vec.Normalize();
        //}
        //移動量を算出する
        vec *= movespeed * Time.deltaTime;
        //実際にプレイヤーを動かす
        //rb.linearVelocityX = vec.x;
        transform.Translate(vec.x, 0.0f, 0.0f);
        // 入力方向を向く角度を計算 (2DなのでZ軸回転)
        float angle = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

        // 現在の回転から、計算した角度へ徐々に回転させる
        Arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);
        //Vector2 arrowangletemp = new Vector2 (vec.x - Arrow.transform.position.x,vec.y - Arrow.transform.position.y);
        //float arrowangle = Mathf.Atan2(arrowangletemp.x, arrowangletemp.y) * Mathf.Rad2Deg - 90f;

    }

    void shot()
    {
        rb.AddForce(Arrow.transform.up * Shotspeed, ForceMode2D.Impulse);
        shotFlag = true;
        shotCount--;
    }
    
    void comborank()
    {
        if (combocount > 49)
        {
            rank = "S";
        }
        else if (combocount > 39)
        {
            rank = "A";
        }
        else if (combocount > 29)
        {
            rank = "B";
        }
        else if (combocount > 19)
        {
            rank = "C";
        }
        else if (combocount > 9)
        {
            rank = "D";
        }
        else
        {
            rank = "none";
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
                //rb.AddForce(-transform.right * 10, ForceMode2D.Impulse);
            }
        }
    }
}
