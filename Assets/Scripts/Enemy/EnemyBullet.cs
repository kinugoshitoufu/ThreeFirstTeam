using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.0f;                   // 移動値
    [SerializeField] Vector3 moveVec = new Vector3(-1, 0, 0);  // 移動方向

    void Update()
    {
        float add_move = moveSpeed * Time.deltaTime;
        transform.Translate(moveVec * add_move);
    }

    public void SetMoveSpeed(float _speed)
    {
        moveSpeed = _speed;
    }

    public void SetMoveVec(Vector3 _vec)
    {
        moveVec = _vec.normalized;
    }
}
