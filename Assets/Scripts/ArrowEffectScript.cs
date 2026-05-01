using UnityEngine;

public class ArrowEffectScript : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.instance.GetShotFlag() == true)
        {
            
        }
    }
}
