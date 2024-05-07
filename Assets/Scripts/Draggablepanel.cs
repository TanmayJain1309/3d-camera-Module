using UnityEngine;
using UnityEngine.EventSystems;

public class Draggablepanel : MonoBehaviour, IDragHandler
{
    private RectTransform panelRectTransform;
    public Canvas canvas;

    void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        panelRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
