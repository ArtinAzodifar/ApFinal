using UnityEngine;
using System.Collections;

public class CanvasToggle : MonoBehaviour
{
    [SerializeField] private RectTransform canvasToShow;
    [SerializeField] private RectTransform canvasToHide;

    public void ToggleCanvases()
    {
        if (canvasToHide == null || canvasToShow == null) return;

        StartCoroutine(AnimateToggle());
    }

    private IEnumerator AnimateToggle()
    {
        UIAnimationManager.Instance.HideWindow(canvasToHide);

        yield return new WaitForSecondsRealtime(0.3f);

        UIAnimationManager.Instance.ShowWindow(canvasToShow);
    }
}