using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public static int KillCount = 0;

    public static void AddKill()
    {
        KillCount++;
    }
}