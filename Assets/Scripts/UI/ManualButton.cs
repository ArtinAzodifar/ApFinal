using UnityEngine;

public class ManualButton : MonoBehaviour
{
    [Header("Canvases to Toggle")]
    [SerializeField] private Canvas canvasToHide;
    [SerializeField] private Canvas canvasToShow;
    
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 1. Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // 2. Check if the mouse position is inside this button's rectangle.
            // For Screen Space - Overlay canvases, the camera parameter must be null.
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null))
            {
                ToggleTheCanvases();
            }
        }
    }

    private void ToggleTheCanvases()
    {
        if (canvasToHide != null)
        {
            canvasToHide.gameObject.SetActive(false);
        }

        if (canvasToShow != null)
        {
            canvasToShow.gameObject.SetActive(true);
        }
    }
}