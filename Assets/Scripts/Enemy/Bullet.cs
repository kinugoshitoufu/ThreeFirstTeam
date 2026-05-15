using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 moveDir;

    public void Init(Vector2 targetPos)
    {
        moveDir = (targetPos - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }
}