using UnityEngine;

public class PlayerScript : MonoBehaviour
{
　　//最低限必須なもの
    public static PlayerScript instance;
    public GameObject Arrow;
    public float movespeed = 3.0f;
    public float Shotspeed = 10.0f;
    public int shotCount = 1;

    //操作によって変更

    //フラグ等
    private Vector3 PlayerPos;
    private bool shotFlag;
    private int control = 0;//操作方法の変更
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        PlayerPos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PlayerPos = transform.position;

    }

    void Move()
    {
        //スティック入力をVecter2型に代入
        Vector2 vec = new Vector2(0,0);
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");
        // ベクトルの長さが１を超えていたら長さを１にする
        //if (vec.sqrMagnitude > 1)
        //{
        //    vec.Normalize();
        //}
        //移動量を算出する
        vec *= movespeed;
        //実際にプレイヤーを動かす
        rb.linearVelocityX = vec.x;
        Vector3 diff = transform.position - PlayerPos; //プレイヤーがどの方向に進んでいるかがわかるように、初期位置と現在地の座標差分を取得

        if (diff.magnitude > 0.01f) //ベクトルの長さが0.01fより大きい場合にプレイヤーの向きを変える処理を入れる(0では入れないので）
        {
            Arrow.transform.rotation = Quaternion.LookRotation(diff);  //ベクトルの情報をQuaternion.LookRotationに引き渡し回転量を取得しプレイヤーを回転させる
        }
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
            
        }        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            
        }
    }
}
