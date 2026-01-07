using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButton : ButtonUI, IPointerDownHandler, IPointerUpHandler
{
    public static int SelectValue { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectValue = 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SelectValue = 0;
    }
}