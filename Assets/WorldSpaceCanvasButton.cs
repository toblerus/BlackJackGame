using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldSpaceCanvasButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent onClick;
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    private Image image;
    private Color originalColor;
    private bool isHovered = false;

    void Start()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            originalColor = image.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovered)
        {
            isHovered = true;
            onHoverEnter.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovered)
        {
            isHovered = false;
            onHoverExit.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }
}
