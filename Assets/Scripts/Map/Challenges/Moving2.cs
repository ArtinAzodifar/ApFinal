using System;
using UnityEngine;

public class Moving2 : MonoBehaviour
{
    [SerializeField] private Transform PointA;
    [SerializeField] private Transform PointB;
    [SerializeField] private float speed;
    
    private Vector3 nexPos;

    private void Start()
    {
        nexPos = PointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nexPos, speed * Time.deltaTime);

        if (transform.position == nexPos)
        {
            nexPos = (nexPos == PointA.position) ? PointB.position : PointA.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
