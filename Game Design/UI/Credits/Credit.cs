using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Credit : MonoBehaviour
{
    [SerializeField] public RectTransform uiElement;
    protected const float TITLE_SIZE = 150f;
    protected const float SUBTITLE_SIZE = 125f;
    protected const float TEXT_SIZE = 100f;

    public abstract void UpdateText(CreditInformation creditInformation);
}