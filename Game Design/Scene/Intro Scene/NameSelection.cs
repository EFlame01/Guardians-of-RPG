using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class NameSelection : MonoBehaviour
{
    [SerializeField] private Button[] letters;
    [SerializeField] private TextMeshProUGUI nameSpace;

    private bool shiftOn = true;

    public void OnEnable()
    {
        nameSpace.text = "__________";
        IntroScene.PlayerName = "";
        IntroScene.CurrentStory.variablesState["stateStatus"] = "";
    }

    public void OnLetterPressed(string letter)
    {
        if (IntroScene.PlayerName.Length >= 10)
            return;

        if (shiftOn && letter != " ")
            letter = letter.ToUpper();
        else if (!shiftOn && letter != " ")
            letter = letter.ToLower();

        IntroScene.PlayerName += letter;

        nameSpace.text = IntroScene.PlayerName + string.Concat(Enumerable.Repeat("_", 10 - IntroScene.PlayerName.Length));

        if (IntroScene.PlayerName.Length > 0 && ContainsLetter())
        {
            IntroScene.CurrentStory.variablesState["stateStatus"] = "next";
        }
        else
            IntroScene.CurrentStory.variablesState["stateStatus"] = "";

        IntroScene.CurrentStory.variablesState["playerName"] = IntroScene.PlayerName;
    }

    public void OnSpacePressed()
    {
        if (IntroScene.PlayerName.Length >= 10)
            return;
        OnLetterPressed(" ");
    }

    public void OnShiftPressed()
    {
        shiftOn = !shiftOn;
        foreach (Button button in letters)
        {
            TextMeshProUGUI textMeshProComponent = button.GetComponentInChildren<TextMeshProUGUI>();
            if (shiftOn)
                textMeshProComponent.text = textMeshProComponent.text.ToUpper();
            else
                textMeshProComponent.text = textMeshProComponent.text.ToLower();
        }
    }

    public void OnBackSpacePressed()
    {
        if (IntroScene.PlayerName.Length == 0)
            return;

        IntroScene.PlayerName = IntroScene.PlayerName.Substring(0, IntroScene.PlayerName.Length - 1);

        nameSpace.text = IntroScene.PlayerName;

        for (int i = IntroScene.PlayerName.Length; i < 10; i++)
            nameSpace.text += "_";

        if (IntroScene.PlayerName.Length == 0)
            IntroScene.CurrentStory.variablesState["stateStatus"] = "";
    }

    private bool ContainsLetter()
    {
        foreach (char letter in IntroScene.PlayerName)
        {
            if (char.IsLetterOrDigit(letter))
                return true;
        }

        return false;
    }
}