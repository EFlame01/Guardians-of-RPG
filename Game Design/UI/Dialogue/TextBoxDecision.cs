using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// </summary>
public class TextBoxDecision : TextBox
{
    public Button DecisionOptionPrefab;
    public Transform ListLayout;
    public int OptionSelected = -1;

    public List<Choice> choices;

    public override void Update()
    {
        if (Select.action.ReadValue<float>() <= 0f)
            return;
        if (!_textBoxOpened)
            return;
        if (GameManager.Instance.EnableNarrationInputs)
        {
            ChooseFirstOption();
        }
    }

    public void UpdateOptionButton(Button button, string optionString)
    {
        TextMeshProUGUI optionText = button.GetComponentInChildren<TextMeshProUGUI>();
        optionText.text = optionString;
    }

    public void SelectOption(int option)
    {
        OptionSelected = option;
    }

    private void ChooseFirstOption()
    {
        SelectOption(0);
        DialogueManager.Instance.MakeDecision(choices[OptionSelected], choices);
    }
}