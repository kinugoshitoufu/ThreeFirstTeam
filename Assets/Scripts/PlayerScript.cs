using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;
    public GameObject Arrow;
    public float movespeed = 3.0f;
    public float Shotspeed = 10.0f;
    public float jumppower = 1.0f;
    public int jumpshotCount = 2;

    private bool jumpshotFlag;
    private bool jumpFlag;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space) && jumpFlag == true)
        {
            Jump();
        }
    }

    void Move()
    {
        //スティック入力をVecter2型に代入
        Vector2 vec = new Vector2(0,0);
        vec.x = Input.GetAxisRaw("Horizontal");
        //vec.y = Input.GetAxisRaw("Vertical");
        // ベクトルの長さが１を超えていたら長さを１にする
        if (vec.sqrMagnitude > 1)
        {
            vec.Normalize();
        }
        //移動量を算出する
        vec *= movespeed * Time.deltaTime;
        //実際にプレイヤーを動かす
        transform.Translate(vec);
    }

    void Jump()
    {
        rb.AddForceY(jumppower,ForceMode2D.Impulse);    
        jumpshotCount--;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            jumpFlag = true;
        }        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            jumpFlag = false;
        }
    }
}
