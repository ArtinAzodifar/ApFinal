using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float speed = 2f;

    protected Vector3 startPos;
    protected Vector3 endPosVertical;
    protected bool shouldMove = false;
    
    [SerializeField] private List<GameObject> enemies;

    protected void Start()
    {
        startPos = transform.position;

        float height = GetComponent<CompositeCollider2D>().bounds.size.y;
        endPosVertical = startPos + new Vector3(0, height, 0);
    }
    
    protected virtual void Update()
    {
        if (!shouldMove && AllEnemiesDead())
        {
            OpenDoor();
        }

        if (shouldMove)
        {
            VerticalMove();
        }
    }

    protected void VerticalMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosVertical, speed * Time.deltaTime);

        if (transform.position == endPosVertical)
        {
            shouldMove = false;
        }
    }

    private void OpenDoor()
    {
        shouldMove = true;
    }
    
    private bool AllEnemiesDead()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && enemy.activeInHierarchy)
                return false;
        }
        return true; 
    }
}