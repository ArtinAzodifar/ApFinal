using System;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    [SerializeField] private GameObject key;

    void Start()
    {
        if (key != null)
        {
            key.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ArrowPool"))
        {
            if (key != null)
            {
                key.SetActive(true);
            }
        }
    }
}
