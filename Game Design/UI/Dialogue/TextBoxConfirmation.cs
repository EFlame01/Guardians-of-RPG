using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// </summary>
public class TextBoxConfirmation : TextBox
{
    public Button ConfirmButton;
    public Button CancelButton;

    public override void Update()
    {
        if (Select.action.ReadValue<float>() <= 0f)
            return;
        if (!_textBoxOpened)
            return;
        if (GameManager.Instance.EnableNarrationInputs)
            ClickConfirm();
    }
}