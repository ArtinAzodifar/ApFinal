using UnityEngine;

public class AutoCameraMover : MonoBehaviour
{
    public Camera cam;           // Assign manually or will auto-assign
    public Vector2 direction = Vector2.right; // Move to the right by default
    public float speed = 5f;     // Units per second

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        Vector3 move = new Vector3(direction.x, direction.y, 0f) * (speed * Time.deltaTime);
        cam.transform.position += move;
    }
}