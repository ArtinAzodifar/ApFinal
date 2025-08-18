using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIGridFiller : MonoBehaviour
{
    [Tooltip("The UI Panel or Image that will act as the container for the grid.")]
    public RectTransform container;

    [Tooltip("The sprite to use for each generated UI Image.")]
    public Sprite imageSprite;
    
    [Tooltip("The desired pixel dimensions for each individual image in the grid.")]
    public Vector2 imageSize = new Vector2(100, 100);

#if UNITY_EDITOR
    [ContextMenu("Clear And Fill UI Grid")]
    private void GenerateGrid()
    {
        if (container == null || imageSprite == null)
        {
            Debug.LogError("Please assign the Container RectTransform and the Image Sprite in the inspector!");
            return;
        }

        if (imageSize.x <= 0 || imageSize.y <= 0)
        {
            Debug.LogError("Image Size must have positive dimensions!");
            return;
        }
        
        // --- 1. Clear existing images inside the container ---
        // We iterate backwards because the collection changes as we destroy items.
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            // Use DestroyImmediate in the editor, as Destroy is delayed.
            DestroyImmediate(container.GetChild(i).gameObject);
        }

        // --- 2. Calculate grid dimensions ---
        Rect containerRect = container.rect;
        int columns = Mathf.FloorToInt(containerRect.width / imageSize.x);
        int rows = Mathf.FloorToInt(containerRect.height / imageSize.y);

        if (columns == 0 || rows == 0)
        {
            Debug.LogWarning("The container is too small to fit even one image of the specified size.");
            return;
        }

        // --- 3. Create and position new images ---
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // Create a new GameObject with an Image component
                GameObject imageObject = new GameObject($"GridImage_{c}_{r}");
                Image imageComponent = imageObject.AddComponent<Image>();
                imageComponent.sprite = imageSprite;

                // Set it as a child of the container
                RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
                rectTransform.SetParent(container, false);

                // --- Configure the RectTransform for grid layout ---
                // Anchor to the top-left of the container
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                
                // Set the pivot to its own top-left corner
                rectTransform.pivot = new Vector2(0, 1);

                // Set the size of the image
                rectTransform.sizeDelta = imageSize;
                
                // Calculate the position based on column and row
                float xPos = c * imageSize.x;
                float yPos = -r * imageSize.y; // Y is negative because we start from the top
                rectTransform.anchoredPosition = new Vector2(xPos, yPos);
            }
        }
        
        EditorUtility.SetDirty(container);
        Debug.Log($"Successfully placed {rows * columns} images in a {columns}x{rows} grid.");
    }
#endif
}