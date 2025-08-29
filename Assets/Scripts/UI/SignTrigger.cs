using UnityEngine;

public class SignTrigger : MonoBehaviour
{
    public RectTransform signWindow;

    void Start()
    {
        // Make sure the window is hidden initially
        if (signWindow != null)
        {
            signWindow.localScale = Vector3.zero;
            signWindow.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            UIAnimationManager.Instance.ShowWindow(signWindow);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            UIAnimationManager.Instance.HideWindow(signWindow);
        }
    }
}