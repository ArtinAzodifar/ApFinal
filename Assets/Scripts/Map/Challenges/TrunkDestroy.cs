using System.Collections;
using UnityEngine;

public class TrunkDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            StartCoroutine(DestroyTrunk());
        }
    }
    
    IEnumerator DestroyTrunk()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
