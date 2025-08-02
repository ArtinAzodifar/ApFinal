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
        originalColor = buttonOutline.effectColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonOutline.effectColor = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonOutline.effectColor = originalColor;
    }
}