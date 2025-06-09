using UnityEngine;

public class CanvasToggle : MonoBehaviour
{
    [SerializeField] private Canvas otherCanvas;
    [SerializeField] private Canvas currentCanvas;

    public void ToggleCanvases()
    {
        if (currentCanvas == null || otherCanvas == null) return;

        bool isCurrentActive = currentCanvas.gameObject.activeSelf;
        bool isOtherActive = otherCanvas.gameObject.activeSelf;

        currentCanvas.gameObject.SetActive(!isCurrentActive);
        otherCanvas.gameObject.SetActive(!isOtherActive);
    }
}