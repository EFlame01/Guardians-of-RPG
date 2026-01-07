using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButton : ButtonUI, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public string XorY;
    [SerializeField] public int Value;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (XorY == "X")
            InputHandler._velocity.x = Value;
        else if (XorY == "Y")
            InputHandler._velocity.y = Value;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (XorY == "X")
            InputHandler._velocity.x = 0;
        else if (XorY == "Y")
            InputHandler._velocity.y = 0;
    }

}