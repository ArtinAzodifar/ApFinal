using UnityEngine;
using Unity.Netcode;

public class Chunk2_1 : NetworkBehaviour,LeverToggle
{
    [SerializeField] private moving1 m;
    private bool isPressed = false;

    public void Toggle()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        if (!isPressed)
        {
            isPressed = true;
            m.setMovingToStart(true);
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
