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
    }
    
    void Shot()
    {
        //Debug.Log("startFlag:" + startFlag);
        //Debug.Log("isFlag:" + isFlag);
        if (!isFlag&& !startFlag) return;
        
        Debug.Log(playerObj);
        shotFrame += Time.deltaTime;
        if (shotFrame > shotData.frame)
        {
            switch (shotData.type)
            {
                // プレイヤーを狙う
                case ShotType.NORMAL:
                    {
                        if (playerObj == null) { break; }
                        EnemyBullet bullet = (EnemyBullet)Instantiate(
                            shotData.bullet,
                            transform.position,
                            Quaternion.identity
                        );
                        //bullet.SetMoveVec(playerObj.transform.position - transform.position);
                        //bullet.SetMoveVec(dir);

                    }
                    break;
            }
            shotFrame = 0;
        }    
    }
    
    void Update()
    {
        Shot();
        
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
            //Die(enemyDeath2Animator,);
        }
    }

    private void ShotStartEnemy()
    {
        this.startFlag = true;
        this.tag = "Enemy";
        Debug.Log(startFlag);
    }
    
}