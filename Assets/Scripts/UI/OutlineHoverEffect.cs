using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
public class OutlineHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline buttonOutline;
    private Color originalColor;

    void Awake()
    {
        buttonOutline = GetComponent<Outline>();
        if (buttonOutline != null)
        {
            originalColor = buttonOutline.effectColor;
        }
    }

    void OnEnable()
    {
        if (buttonOutline != null)
        {
            buttonOutline.effectColor = originalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonOutline != null)
        {
            buttonOutline.effectColor = Color.white;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonOutline != null)
        {
            buttonOutline.effectColor = originalColor;
        }
    }
}