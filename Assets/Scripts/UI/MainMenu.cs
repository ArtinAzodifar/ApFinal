using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    public Canvas otherCanvas;
    private Canvas currentCanvas;

    void Start()
    {
        currentCanvas = GetComponentInParent<Canvas>();
    }

    public void ToggleCanvases()
    {
        if (currentCanvas == null || otherCanvas == null) return;

        bool isCurrentActive = currentCanvas.gameObject.activeSelf;
        bool isOtherActive = otherCanvas.gameObject.activeSelf;

        currentCanvas.gameObject.SetActive(!isCurrentActive);
        otherCanvas.gameObject.SetActive(!isOtherActive);
    }
}