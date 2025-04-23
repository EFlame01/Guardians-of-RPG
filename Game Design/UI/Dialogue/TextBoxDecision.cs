using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// </summary>
public class TextBoxDecision : TextBox
{
    [SerializeField] public Button DecisionOptionPrefab;
    [SerializeField] public Transform ListLayout;
    public int OptionSelected = -1;

    public void UpdateOptionButton(Button button, string optionString)
    {
        TextMeshProUGUI optionText = button.GetComponentInChildren<TextMeshProUGUI>();
        optionText.text = optionString;
    }

    public void SelectOption(int option)
    {
        OptionSelected = option;
    }
}