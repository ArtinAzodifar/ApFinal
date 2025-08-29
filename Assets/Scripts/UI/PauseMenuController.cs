using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class PauseMenuItem
{
    public TextMeshProUGUI textMesh;
    public Image highlightImage;
}

public class PauseMenuController : MonoBehaviour
{
    [Header("Menu Items")]
    [SerializeField] private PauseMenuItem[] menuItems;

    [Header("Selection Visuals")]
    [SerializeField] private Color normalColor = Color.clear;
    [SerializeField] private Color selectedColor = Color.blue;
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1.1f);
    
    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.1f;
    private Coroutine[] activeCoroutines;

    private int currentItemIndex = 0;

    private void Awake()
    {
        activeCoroutines = new Coroutine[menuItems.Length];
    }
    
    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Navigate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Navigate(1);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmSelection();
        }
    }
    
    public void ShowMenu()
    {
        gameObject.SetActive(true);
        currentItemIndex = 0;
        UpdateSelectionVisuals();
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }
    
    private void Navigate(int direction)
    {
        currentItemIndex += direction;
        if (currentItemIndex < 0) currentItemIndex = menuItems.Length - 1;
        else if (currentItemIndex >= menuItems.Length) currentItemIndex = 0;
        UpdateSelectionVisuals();
    }

    public void OnMenuItemHover(int index)
    {
        if (currentItemIndex == index) return;
        currentItemIndex = index;
        UpdateSelectionVisuals();
    }

    private void UpdateSelectionVisuals()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            Vector3 targetScale = (i == currentItemIndex) ? selectedScale : normalScale;
            Color targetColor = (i == currentItemIndex) ? selectedColor : normalColor;
            if (activeCoroutines[i] != null) StopCoroutine(activeCoroutines[i]);
            activeCoroutines[i] = StartCoroutine(AnimateScale(menuItems[i].textMesh.transform, targetScale, animationDuration));
            menuItems[i].highlightImage.color = targetColor;
        }
    }

    private IEnumerator AnimateScale(Transform targetTransform, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = targetTransform.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            targetTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        targetTransform.localScale = targetScale;
    }
    
    private void ConfirmSelection()
    {
        switch (currentItemIndex)
        {
            case 0: // Resume
                GameManager.Instance.ResumeGame();
                break;
            case 1: // Restart
                GameManager.Instance.Restart();
                break;
            case 2: // Options
                Debug.Log("Options Selected!");
                break;
            case 3: // Main Menu
                GameManager.Instance.MainMenu(true);
                break;
        }
    }
}