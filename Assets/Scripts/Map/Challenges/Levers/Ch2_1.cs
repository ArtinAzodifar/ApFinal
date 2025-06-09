using UnityEngine;

public class Chunk2_1 : MonoBehaviour,LeverToggle
{
    [SerializeField] private moving1 m;
    private bool isPressed = false;

    public void Toggle()
    {
        if (!isPressed)
        {
            isPressed = true;
            m.setMovingToStart(true);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
