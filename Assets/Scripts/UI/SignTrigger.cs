using UnityEngine;
using DG.Tweening;

public class SignTrigger : MonoBehaviour
{
    public RectTransform windowRectTransform;

    // Awake is called before the first frame update
    void Awake()
    {
        // Ensure the window is assigned
        if (windowRectTransform != null)
        {
            // Start with the window hidden (scaled to zero)
            windowRectTransform.localScale = Vector3.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Player1") || other.CompareTag("Player2")) && windowRectTransform != null)
        {
            // Kill any previous animation and scale up to 1
            windowRectTransform.DOKill();
            windowRectTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.CompareTag("Player1") || other.CompareTag("Player2")) && windowRectTransform != null)
        {
            // Kill any previous animation and scale down to 0
            windowRectTransform.DOKill();
            windowRectTransform.DOScale(0f, 0.3f).SetEase(Ease.InQuad);
        }
    }
}