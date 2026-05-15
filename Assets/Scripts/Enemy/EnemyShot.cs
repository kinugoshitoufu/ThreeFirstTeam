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

    GameObject playerObj = null;//プレイヤーオブジェクト
    int shotFrame = 0;          //フレーム
    public float lifeTimer = 0f;
    void Start()
    {
        //プレイヤオブジェクトを取得する
        switch (shotData.type)
        {
            case ShotType.NORMAL:
                playerObj = GameObject.Find("Player");
                break;
        }
    }

    void Shot()
    {
        Vector2 dir = (playerObj.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        shotFrame++;
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
                            Quaternion.Euler(0,0,angle)
                        );
                        //bullet.SetMoveVec(playerObj.transform.position - transform.position);
                        bullet.SetMoveVec(dir);

                    }
                    break;
            }
            shotFrame = 0;
        }    
    }
    
    void Update()
    {
        Shot();

        lifeTimer += Time.deltaTime;
        if (lifeTimer > 5f)
        {
            SoundManager.Instance.GAMESE(1);
            Die();
        }
    }
}
