using System.Collections;
using UnityEngine;

public class TrunkDestroy : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            StartCoroutine(DestroyTrunk());
        }
    }
    
    IEnumerator DestroyTrunk()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
