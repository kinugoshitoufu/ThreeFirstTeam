using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
　　//最低限必須なもの
    public static PlayerScript instance;
    public GameObject Arrow;
    public float movespeed = 3.0f;
    public float Shotspeed = 10.0f;
    public float shottime = 3.0f;
    public float gravity = 1.0f;
    public int shotCount = 1;

    //操作によって変更

    //フラグ等
    private Vector3 PlayerPos;
    private bool shotFlag;
    public float shottimecount = 0.0f;
    private int control = 0;//操作方法の変更
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        PlayerPos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody2D>();
        shotFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shotFlag)
        {
            Move();
        }
        PlayerPos = transform.position;
        if (Input.GetKeyDown("joystick button 0") && shotCount > 0 && !shotFlag)
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
    
    public Vector2 HoriVert()
    {
        Vector2 vec = new Vector2(0, 0);
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");
        return vec;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            //if (shotCount < 1)
            //{
            //    shotCount = 1;
            //}
        }        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (shotCount < 1)
            {
                shotCount = 1;
            }
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
                shotCount++;
            }
            else
            {
                //rb.AddForce(-transform.right * 10, ForceMode2D.Impulse);
            }
        }
    }
}
