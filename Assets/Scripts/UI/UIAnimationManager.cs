using UnityEngine;
using DG.Tweening;

public class UIAnimationManager : MonoBehaviour
{
    public static UIAnimationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ShowWindow(RectTransform window)
    {
        if (window == null) return;
    
        window.gameObject.SetActive(true);
        window.DOKill();
    
        window.DOScale(1f, 0.5f).From(0f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void HideWindow(RectTransform window)
    {
        if (window == null) return;
    
        window.DOKill();
        window.DOScale(0f, 0.3f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            window.gameObject.SetActive(false);
        }).SetUpdate(true);
    }
}