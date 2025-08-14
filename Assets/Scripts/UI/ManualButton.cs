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
        if (Input.GetMouseButtonDown(0))
        {
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