using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TouchPad : MonoBehaviour
{
    [SerializeField] public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        CheckTouchPadEnabled();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouchPadEnabled();
    }

    private void CheckTouchPadEnabled()
    {
        canvasGroup.interactable = GameManager.Instance.EnableTouchPad;
        if (canvasGroup.interactable)
            canvasGroup.alpha = 0.25f;
        else
            canvasGroup.alpha = 0f;
    }
}
