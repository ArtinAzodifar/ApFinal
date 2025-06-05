using UnityEngine;

public class Door : MonoBehaviour
{
    public float speed = 2f;

    private Vector3 startPos;
    private Vector3 endPosVertical;
    public bool shouldMove = false;

    void Start()
    {
        startPos = transform.position;

        float height = GetComponent<CompositeCollider2D>().bounds.size.y;
        endPosVertical = startPos + new Vector3(0, height, 0);
    }

    void Update()
    {
        if (shouldMove)
        {
            VerticalMove();
        }
    }

    private void VerticalMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosVertical, speed * Time.deltaTime);

        if (transform.position == endPosVertical)
        {
            shouldMove = false;
        }
    }

    public void OpenDoor()
    {
        shouldMove = true;
    }
}