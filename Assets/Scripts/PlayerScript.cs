using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SEData
{
    public AudioClip clip;

    [Range(0f, 1f)]
    public float SEvolume = 1f;
}
public class PlayerScript : MonoBehaviour
{
    //パラメーターなど
    public static PlayerScript instance;
    public GameObject Arrow;
    public Animator Arrowanimator;
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
    public int combocountRank = 10;
    public float timedown = 3.0f;
    public GameObject particle;
    public int HitStopFlame = 3;
    public float ControllerDeadZone = 0.1f;
    public float DamageTime = 0.5f;
    public SEData[] audios;

    //操作によって変更
    private int control = 0;//操作方法の変更

    //フラグ・カウント系など
    [SerializeField] private string rank = "none";
    private Animator animator;
    private BoxCollider2D box;
    private SpriteRenderer spriteRenderer;
    private OnFlashScript onFlashScript;
    private AudioSource audioSource;
    private Vector2 BoxSize;
    private Vector3 PlayerScale;
    private int combocount = 0;
    private bool shotboxflag = true;
    private int score = 0;
    private bool shotFlag;
    private bool meshFlag = true;
    private float DamageTimeCount = 0.0f;
    private bool DamageFlag;
    public int maxcombo = 0;
    [SerializeField] private float shottimecount = 0.0f;
    [SerializeField] private float combotimecount = 0.0f;
    private Rigidbody2D rb;
    public static bool isMove=false;
   public EnemySpawner1 enemyspawner;
    public float fallSpeed=0.0f;
    public bool isFalling=false;

    //川本こうせいが追加した変数
    public GameObject startEnemy;

    public enum PlayerState
    {
        start,
        Playering,
    }
    public PlayerState playerState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        onFlashScript = GetComponent<OnFlashScript>();
        shotFlag = false;
        shotboxflag = true;
        animator = GetComponent<Animator>();
        BoxSize = box.size;
        PlayerScale = transform.localScale;
        playerState= PlayerState.start;
        isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
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
            if (Input.GetKeyDown(KeyCode.I))
            {
                meshFlag = !meshFlag;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                isMove = !isMove; 
            }
        }
        MoveAreaCheck();
        if (playerState == PlayerState.start)
        {
                StartMove();
                if(isMove)
                {
                    playerState = PlayerState.Playering;
                }
        }
        else if (playerState == PlayerState.Playering)
        {
            if (!shotFlag)
            {
                Move();
            }
        }
        if (meshFlag)
        {
            TriangleMesh.instance.vec1 = transform.position;
            TriangleMesh.instance.vec1.x -= -0.45622f;
            TriangleMesh.instance.vec1.y -= -0.905f;
        }
        if (shotCount > 0 && !shotFlag && !animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDamage"))
        {
            if (DamageTimeCount <= 0.0f)
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
        }
        if (shotFlag)
        {
            shottimecount += Time.deltaTime;
            rb.gravityScale = 0.0f;
        }
        if (shottime < shottimecount)
        {
            shotFlag = false;
            animator.SetBool("ShootingAnim", false);
            rb.linearVelocity = Vector3.zero;
            rb.gravityScale = gravity;
            transform.rotation = Quaternion.identity;
            shottimecount = 0.0f;
        }
        if (DamageFlag)
        {
            DamageTimeCount += Time.deltaTime;
        }
        if (DamageTimeCount > DamageTime)
        {
            DamageFlag = false;
            onFlashScript.EndBlink();
            DamageTimeCount = 0.0f;
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
            box.size = new Vector2(BoxSize.x, BoxSize.y);
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
        if(isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            return;
        }
    }

    void Move()
    {
        //スティック入力をVecter2型に代入
        Vector2 vec;
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");

        // 入力方向を向く角度を計算 (2DなのでZ軸回転)
        float angle = Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg;

        // 現在の回転から、計算した角度へ徐々に回転させる
        Arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);

        if (Mathf.Abs(vec.x) <= ControllerDeadZone)
        {
            animator.SetBool("WalkAnim", false);
            vec = Vector2.zero;
        }
        else
        {
            //移動量を算出する
            animator.SetBool("WalkAnim", true);
            vec *= movespeed * Time.deltaTime;
        }
        //実際にプレイヤーを動かす
        if (vec.x < 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (vec.x > 0f)
        {
            spriteRenderer.flipX = true;
        }
        //rb.linearVelocityX = vec.x;
        transform.Translate(vec.x, 0.0f, 0.0f);
        //Vector2 arrowangletemp = new Vector2 (vec.x - Arrow.transform.position.x,vec.y - Arrow.transform.position.y);
        //float arrowangle = Mathf.Atan2(arrowangletemp.x, arrowangletemp.y) * Mathf.Rad2Deg - 90f;

    }

    void StartMove()
    {
        //スティック入力をVecter2型に代入
        Vector2 Startvec;
        Startvec.x = Input.GetAxisRaw("Horizontal");
        Startvec.y = Input.GetAxisRaw("Vertical");

        // 入力方向を向く角度を計算 (2DなのでZ軸回転)
        float angle = EnemyAngleCheck();

        // 現在の回転から、計算した角度へ徐々に回転させる
        Arrow.transform.rotation = Quaternion.Euler(0, 0, -angle);

        if (Mathf.Abs(Startvec.x) <= ControllerDeadZone)
        {
            animator.SetBool("WalkAnim", false);
        }
        
    }

    float EnemyAngleCheck()
    {
        if (startEnemy == null)
        {
            return 0.0f;
        }
        Vector2 enemy = startEnemy.transform.position;
        Vector2 dir = enemy - (Vector2)transform.position;

        float EnemyAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        
        return EnemyAngle;
    }


    void shot()
    {
        audioSource.PlayOneShot(audios[0].clip,audios[0].SEvolume);
        rb.linearVelocity *= 0.0f;
        rb.AddForce(Arrow.transform.up * Shotspeed, ForceMode2D.Impulse);
        shotFlag = true;
        transform.rotation = Arrow.transform.rotation;
        Arrow.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        animator.SetBool("ShootingAnim", true);
        animator.Play("PlayerShot", 0, 0.0f);
        if (combocount >= combocountRank * 5)
        {
            Arrowanimator.Play("ARankShotEffect", 0, 0.0f);
        }
        else if (combocount >= combocountRank * 4)
        {
            Arrowanimator.Play("BRankShotEffect", 0, 0.0f);
        }
        else if (combocount >= combocountRank * 3)
        {
            Arrowanimator.Play("CRankShotEffect", 0, 0.0f);
        }
        else if (combocount >= combocountRank * 2)
        {
            Arrowanimator.Play("DRankShotEffect", 0, 0.0f);
        }
        else if (combocount >= combocountRank)
        {
            Arrowanimator.Play("ERankShotEffect", 0, 0.0f);
        }
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
            if (rb.linearVelocityY < 0)
            {
                rb.linearVelocityY = 0.0f;
            }
            if (shotCount < 1)
            {
                shotCount = 1;
            }
            //animator.SetBool("FallAnim", false);
            shotboxflag = false;
            //Debug.Log("これ");
            //vec.y = -vec.y;
            //vec *= 0.0f;
        }
        else
        {
            //animator.SetBool("FallAnim", true);
            shotboxflag = true;
        }
        transform.position = pos;
        //rb.linearVelocity = vec;

    }

    void comborank()
    {
        if (combocount >= combocountRank * 5)
        {
            rank = "A";
        }
        else if (combocount >= combocountRank * 4)
        {
            rank = "B";
        }
        else if (combocount >= combocountRank * 3)
        {
            rank = "C";
        }
        else if (combocount >= combocountRank * 2)
        {
            rank = "D";
        }
        else if (combocount >= combocountRank)
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
            box.size = new Vector2(BoxSize.x + (shotcollider * 5), BoxSize.y + (shotcollider * 5));
        }
        else if (rank == "B")
        {
            box.size = new Vector2(BoxSize.x + (shotcollider * 4), BoxSize.y + (shotcollider * 4));
        }
        else if (rank == "C")
        {
            box.size = new Vector2(BoxSize.x + (shotcollider * 3), BoxSize.y + (shotcollider * 3));
        }
        else if (rank == "D")
        {
            box.size = new Vector2(BoxSize.x + (shotcollider * 2), BoxSize.y + (shotcollider * 2));
        }
        else if (rank == "E")
        {
            box.size = new Vector2(BoxSize.x + (shotcollider * 1), BoxSize.y + (shotcollider * 1));
        }
        else
        {
            box.size = new Vector2(BoxSize.x, BoxSize.y);
        }
    }

    public Vector2 GetHoriVert()
    {
        Vector2 vec = new Vector2(0, 0);
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");
        return vec;
    }

    public bool GetShotFlag()
    {
        return shotFlag;
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
            //animator.SetBool("FallAnim",false);
            shotboxflag = false;
            //transform.position = new Vector3(transform.position.x,transform.position.y - (box.size.y - 1.0f));
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isFalling && collision.collider.CompareTag("Ground"))
        {
            isFalling = false;
            ResultScript.instance.ShowResult();
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
            //animator.SetBool("FallAnim", true);
            shotboxflag = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            animator.SetBool("FallAnim", false);
        }
        if (collision.CompareTag("Enemy"))
        {
            if (shotFlag)
            {
                //  shaking();
                Destroy(collision.gameObject);
                audioSource.PlayOneShot(audios[2].clip, audios[2].SEvolume);
                combocount++;
                shotCount++;
                combotimecount = 0.0f;
                score += scorecount * combocount;
                Instantiate(particle, transform.position, Quaternion.identity);
                gravity -= fallspeed / downfallspeed;
                Timescript.LimitTime += timeup;
            }
            else
            {
                //rb.AddForce(-Arrow.transform.up * knockback, ForceMode2D.Impulse);
                if (DamageTimeCount <= 0.0f)
                {
                    Timescript.LimitTime -= timedown;
                    audioSource.PlayOneShot(audios[1].clip, audios[1].SEvolume);
                    onFlashScript.BeginBlink();
                    DamageFlag = true;
                }
            }
        }
        if (collision.CompareTag("StartEnemy"))
        {
            StartEnemy startEnemy = collision.gameObject.GetComponent<StartEnemy>();

            if (startEnemy==null)
            {
                return;
            }
            else
            {
                if (shotFlag)
                {
                    //  shaking();
                    Destroy(collision.gameObject);
                    audioSource.PlayOneShot(audios[2].clip, audios[2].SEvolume);
                    combocount++;
                    shotCount++;
                    combotimecount = 0.0f;
                    score += scorecount * combocount;
                    Instantiate(particle, transform.position, Quaternion.identity);
                    gravity -= fallspeed / downfallspeed;
                    Timescript.LimitTime += timeup;
                }
                else
                {
                    //rb.AddForce(-Arrow.transform.up * knockback, ForceMode2D.Impulse);
                    if (DamageTimeCount <= 0.0f)
                    {
                        Timescript.LimitTime -= timedown;
                        audioSource.PlayOneShot(audios[1].clip, audios[1].SEvolume);
                        onFlashScript.BeginBlink();
                        DamageFlag = true;
                    }
                }
                startEnemy.FastEnemyDead();
                isMove = true;
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
                if (DamageTimeCount <= 0.0f)
                {
                    Timescript.LimitTime -= timedown;
                    audioSource.PlayOneShot(audios[1].clip, audios[1].SEvolume);
                    onFlashScript.BeginBlink();
                    DamageFlag = true;
                }
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            animator.SetBool("FallAnim", true);
        }
    }

}
