using System;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GameObject.Find("Trunk").GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ArrowPool"))
        {
            rb.gravityScale = 1;
        }
    }
}
