using System;
using UnityEngine;

public class EnemyShot : Enemy
{
    enum ShotType
    {
        NONE = 0,
        NORMAL = 1,
    }

    [System.Serializable]
    struct ShotData
    {
        public int frame;
        public ShotType type;
        public EnemyBullet bullet;
    }
    
    //ショットデータ
    [SerializeField] ShotData shotData = new ShotData { frame = 60, type = ShotType.NONE, bullet = null };

    public GameObject playerObj;//プレイヤーオブジェクト
    float shotFrame = 0;          //フレーム
    public float lifeTimer = 0f;
    public Animator enemyDeath2Animator;
    private Combo comboManagerShot;
    //private bool startFlag = false;
    void Start()
    {
        //プレイヤオブジェクトを取得する
        //switch (shotData.type)
        //{
        //    case ShotType.NORMAL:
        //        playerObj = GameObject.Find("Player1");
        //        break;
        //}
        comboManagerShot = FindAnyObjectByType<Combo>();
    }
    
    void Shot()
    {
        if (!FeverManeger.IsFever)
        {
            //Debug.Log("startFlag:" + startFlag);
            //Debug.Log("isFlag:" + isFlag);
            if (!isFlag && !startFlag) return;

            shotFrame += Time.deltaTime;
            if (shotFrame > shotData.frame)
            {
                switch (shotData.type)
                {
                    // プレイヤーを狙う
                    case ShotType.NORMAL:
                        {
                            Debug.Log("生成開始!!");
                            if (playerObj == null) { break; }
                            EnemyBullet bullet = (EnemyBullet)Instantiate(
                                shotData.bullet,
                                transform.position,
                                Quaternion.identity
                            );
                        }
                        break;
                }
                shotFrame = 0;
            }
        }
    }
    
    void Update()
    {
        Shot();
        if(FeverManeger.IsFever)
        {
            Bullet.bulletspeed = 0f;
        }
        else if(!FeverManeger.IsFever)
        {
            Bullet.bulletspeed = Bullet.defaultSpeed;
        }

        //lifeTimer += Time.deltaTime;
        //if (lifeTimer > 5f)
        //{
        //    Die();
        //}
    }
    public new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && startFlag)
        {
            comboManagerShot.SetEnemyPos(transform.position);
            Die();
        }
    }

    private void ShotStartEnemy()
    {
        this.startFlag = true;
        this.tag = "Enemy";
        Debug.Log(startFlag);
    }
    
}