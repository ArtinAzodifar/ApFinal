using Unity.VisualScripting;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public void Delete()
    {
        Animator  animator = GetComponent<Animator>();
        animator.SetTrigger("Delete");
        Destroy(gameObject, 0.3f);
    }
}
