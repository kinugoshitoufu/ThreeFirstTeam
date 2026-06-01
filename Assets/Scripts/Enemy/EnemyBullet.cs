using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] 
    float moveSpeed = 3.0f;     // 移動値
    Vector2 moveVec;            // 移動方向
    private float bulletTimer = 0.1f;
    public float bulletDethCount = 0.3f;
    public GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Debug.Log(player.transform.position);
        Vector2 dir = (player.transform.position - transform.position).normalized;
        moveVec = dir;
        moveVec = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
        transform.position += new Vector3(moveVec.x, moveVec.y, 0) * moveSpeed * Time.deltaTime;
    }

    void Update()
    {
        if (!FeverManeger.IsFever)
        {
            transform.position += (Vector3)(moveVec * moveSpeed * Time.deltaTime);
            bulletTimer += Time.deltaTime;
            if (bulletTimer > bulletDethCount)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void SetMoveSpeed(float _speed)
    {
        moveSpeed = _speed;
    }

    public void SetMoveVec(Vector3 _vec)
    {
        moveVec = new Vector2(_vec.x,_vec.y).normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに当たったよ！！");
            Destroy(gameObject);
        }
    }
    //void LookAt2D(Vector2 targetPosition, bool inversion = true, bool pictyer = false, bool move = false, float speed = 1f, float rotationSpeed = 100f, float slerpFactor = 1f)
    //{
    //    int a = 0;
    //    if (pictyer == true)
    //    {
    //        a = 180;
    //    }
    //    Vector2 myPosition = (Vector2)transform.position;
    //    Vector2 direction = targetPosition - myPosition;
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    Quaternion currentRotation = transform.rotation;
    //    Quaternion targetRotation = Quaternion.Euler(180, a, angle * -1); ;
    //    transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, slerpFactor * Time.deltaTime * rotationSpeed);
    //    if (inversion)
    //    {
    //        if (angle >= 90 || angle <= -90) { transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z); }
    //        else { transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z); }
    //    }
    //    if (move)
    //    {
    //        Vector2 moveDirection = direction.normalized;
    //        Vector2 newPosition = myPosition + moveDirection * speed * Time.deltaTime;
    //        transform.position = newPosition;
    //    }
    //
    //}

}
