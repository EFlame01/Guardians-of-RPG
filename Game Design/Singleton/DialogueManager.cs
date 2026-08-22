using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

/// <summary>
/// Class that inherites from PersistentSingleton class.
/// DialogueManager is responsible for using the DialogueData
/// to diplay the panels required to reveal the dialogue and
/// interact with the story.
/// </summary>
public class DialogueManager : PersistentSingleton<DialogueManager>
{
    //Serialized variables
    public bool DestroyOnLoad;
    public TextBox TextBox;
    public TextBoxConfirmation ConfirmationTextBox;
    public TextBoxDecision DecisionTextBox;
    public TextBox NarrationTextBox;

    //public variables
    public bool DialogueStarted { get; private set; }
    public bool DialogueEnded { get; private set; }
    public bool DialogueContinued { get; private set; }
    public Story CurrentStory;

    //private variables
    private Coroutine _displayTextBoxCoroutine;
    private Coroutine _typeDialogueCoroutine;
    private bool _dialogueEnded;
    private bool _dialogueIsPlaying;
    private DialogueData _dialogueData;
    private TextBox _currentTextBox;
    private string _originalText;
    private int _decision = -1;
    private int _textBoxType = 0;
    private string _itemName = "";
    private string _pluralName = "";
    private string _itemType = "";
    private int _number = 0;
    private bool _clickedAlready = false;

    //Override method added for Intro scene
    //  that may have additional functionality
    //  when using the DialogueManager
    protected override void Awake()
    {
        if (!DestroyOnLoad)
        {
            base.Awake();
        }
        else
        {
            if (Instance == null)
                Instance = this;
        }
    }

    //Getters and Setters
    public void SetTextBox(TextBox textBox)
    {
        TextBox = textBox;
    }
    public int GetDecision()
    {
        return _decision;
    }
    public void ResetDecision()
    {
        _decision = -1;
    }
    public void SetItemName(string itemName)
    {
        _itemName = itemName;
    }
    public void SetPluralName(string pluralName)
    {
        _pluralName = pluralName;
    }
    public void SetItemType(string itemType)
    {
        _itemType = itemType;
    }
    public void SetNumber(int number)
    {
        _number = number;
    }

    private void Start()
    {
        _dialogueIsPlaying = false;
    }

    /// <summary>
    /// Takes a <paramref name="dialogueData"/> variable and uses it
    /// to display the appropriate dialogue.
    /// </summary>
    /// <param name="dialogueData">
    /// The ScriptableObject that will hold the data
    /// for the dialogue
    /// </param>
    public void DisplayNextDialogue(DialogueData dialogueData)
    {
        DialogueEnded = false;
        DialogueContinued = true;
        DialogueStarted = true;

        //assigns global _dialogueData variable to use for further methods
        _dialogueData = dialogueData;

        //if current story does not exist or cannot continue
        if (CurrentStory == null || !CurrentStory.canContinue)
        {
            //this means we have not started dialogue yet
            if (!_dialogueEnded)
            {
                try
                {
                    if (_dialogueData == null)
                    {
                        EndDialogue();
                        return;
                    }
                    CurrentStory = new Story(_dialogueData.InkJSON.text);
                    if (CurrentStory != null)
                    {
                        CurrentStory.ResetState();
                        CurrentStory.onError += HandleStoryError;
                        SetUpDialogueVariables();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning("WARNING: " + e.Message);
                    return;
                }
            }

            //this means we have started dialogue and it has ended
            else if (_dialogueEnded && !_dialogueIsPlaying)
            {
                EndDialogue();
                return;
            }
        }

        //this means we should play the dialogue
        if (!_dialogueIsPlaying)
        {
            try
            {
                _displayTextBoxCoroutine = StartCoroutine(DisplayTextBox());
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error in DisplayNextDialogue(): " + e.Message);
            }
        }

        //if story can no longer continue, set _dialogueEnded to true
        if (!DialogueEnded && (CurrentStory == null || !CurrentStory.canContinue))
            _dialogueEnded = true;
    }

    private IEnumerator DisplayTextBox()
    {
        if (CurrentStory.canContinue)
            _originalText = CurrentStory.Continue();

        _textBoxType = (int)GetVariableState("textBoxType");

        if (_originalText == null || _originalText.Length <= 0)
        {
            if (_textBoxType == 1)
                Debug.LogWarning($"Original text is not present... Ending Dialogue: {_originalText}");
            EndDialogue();
        }
        else
        {
            switch (_textBoxType)
            {
                case Units.ORIGINAL:
                    if (!TextBox.gameObject.activeSelf || TextBox.IsClosed)
                    {
                        CloseRightTextBox(TextBox);
                        TextBox.gameObject.SetActive(true);
                        yield return new WaitForSeconds(0.4f);
                        OpenRightTextBox(TextBox);
                        yield return new WaitForSeconds(0.25f);
                    }
                    _currentTextBox = TextBox;
                    break;
                case Units.NARRATION:
                    if (!NarrationTextBox.gameObject.activeSelf || NarrationTextBox.IsClosed)
                    {
                        CloseRightTextBox(NarrationTextBox);
                        yield return new WaitForSeconds(0.4f);
                        OpenRightTextBox(NarrationTextBox);
                        yield return new WaitForSeconds(0.25f);
                    }
                    _currentTextBox = NarrationTextBox;
                    break;
                case Units.CONFIRMATION:
                    if (!ConfirmationTextBox.gameObject.activeSelf || ConfirmationTextBox.IsClosed)
                    {
                        CloseRightTextBox(ConfirmationTextBox);
                        yield return new WaitForSeconds(0.4f);
                        OpenRightTextBox(ConfirmationTextBox);
                        yield return new WaitForSeconds(0.25f);
                    }
                    SetUpConfirmation();
                    _currentTextBox = ConfirmationTextBox;
                    break;
                case Units.DECISION:
                    if (!DecisionTextBox.gameObject.activeSelf || DecisionTextBox.IsClosed)
                    {
                        CloseRightTextBox(DecisionTextBox);
                        yield return new WaitForSeconds(0.4f);
                        OpenRightTextBox(DecisionTextBox);
                        yield return new WaitForSeconds(0.25f);
                    }
                    SetUpDecision();
                    _currentTextBox = DecisionTextBox;
                    break;
                default:
                    CloseRightTextBox(null);
                    yield return new WaitForSeconds(0.4f);
                    break;
            }

            if (_textBoxType != Units.DECISION)
                _typeDialogueCoroutine = StartCoroutine(TypeDialogue(CurrentStory));
            else
                GameManager.Instance.EnableNarrationInputs = true;
        }//end of else...
    }

    private IEnumerator TypeDialogue(Story story)
    {
        _dialogueIsPlaying = true;
        _currentTextBox.TextMeshComponent.text = "";
        _currentTextBox.ResetScrollBar();
        string displayText;
        int alphaIndex = 0;
        GameManager.Instance.EnableNarrationInputs = false;

        for (int i = 0; i < _originalText.Length; i++)
        {
            alphaIndex++;
            _currentTextBox.TextMeshComponent.text = _originalText;
            displayText = _currentTextBox.TextMeshComponent.text.Insert(alphaIndex, "<color=#00000000>");
            _currentTextBox.TextMeshComponent.text = displayText;
            AudioManager.Instance.PlaySoundEffect("scroll_05");
            yield return new WaitForSeconds(0.02f);
        }

        AudioManager.Instance.StopSoundEffect("scroll_05");

        _dialogueIsPlaying = false;
        DialogueContinued = false;
        DialogueStarted = false;
        GameManager.Instance.EnableNarrationInputs = true;
    }

    public void EndDialogue()
    {
        CurrentStory?.ResetState();
        CurrentStory = null;
        if (_currentTextBox != null)
            _currentTextBox.EndNarration();
        _dialogueEnded = false;
        DialogueEnded = true;
        DialogueContinued = false;
        DialogueStarted = false;
        CloseRightTextBox(null);
        GameManager.Instance.EnableNarrationInputs = false;
    }

    private void FinishEarly()
    {
        try
        {
            StopCoroutine(_typeDialogueCoroutine);
            AudioManager.Instance.StopSoundEffect("scroll_05");
            _currentTextBox.TextMeshComponent.text = CurrentStory.currentText;
            _dialogueIsPlaying = false;
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING: " + e.Message);
        }
    }

    private void SetUpConfirmation()
    {
        List<Choice> choices = CurrentStory.currentChoices;
        _clickedAlready = false;
        ConfirmationTextBox.ConfirmButton.onClick.AddListener(() =>
        {
            ClickedOption(0);
        });

        ConfirmationTextBox.CancelButton.onClick.AddListener(() =>
        {
            ClickedOption(1);
        });
    }

    public void ClickedOption(int index)
    {
        if (_clickedAlready)
            return;

        _clickedAlready = true;
        try
        {
            CurrentStory.ChooseChoiceIndex(index);
            Story tempStory = CurrentStory;
            DisplayNextDialogue(_dialogueData);
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING: " + e.Message);
            EndDialogue();
        }
    }

    private void SetUpDecision()
    {
        List<Choice> choices = CurrentStory.currentChoices;
        _clickedAlready = false;
        if (choices.Count <= 0 || _originalText.Length <= 0)
        {
            Debug.LogWarning("WARNING: Dialiogue should end here.");
            EndDialogue();
            return;
        }

        //Destroy previous options
        foreach (Transform child in DecisionTextBox.ListLayout)
            Destroy(child.gameObject);

        DecisionTextBox.choices = choices;

        foreach (Choice choice in choices)
        {
            Button choiceBtn = Instantiate(DecisionTextBox.DecisionOptionPrefab, DecisionTextBox.ListLayout);
            DecisionTextBox.UpdateOptionButton(choiceBtn, choice.text);
            choiceBtn.onClick.AddListener(() => MakeDecision(choice, choices));
        }
    }

    public void MakeDecision(Choice choice, List<Choice> currentChoices)
    {
        if (_clickedAlready)
            return;

        _clickedAlready = true;

        //Finds the index of the choice based on how many options are in the list
        for (int i = 0; i < currentChoices.Count; i++)
        {
            if (currentChoices[i].text.Equals(choice.text))
            {
                _dialogueIsPlaying = false;
                CurrentStory.ChooseChoiceIndex(i);
                DecisionTextBox.SelectOption(i);
                _decision = i;
                _dialogueIsPlaying = false;
                if (CheckVariableState("endDialogue", "Yes"))
                    CurrentStory.Continue();
                DisplayNextDialogue(_dialogueData);
                break;
            }
        }
    }

    private void CloseRightTextBox(TextBox textBox)
    {
        EndNarrationForTextBox(NarrationTextBox, textBox);
        EndNarrationForTextBox(ConfirmationTextBox, textBox);
        EndNarrationForTextBox(DecisionTextBox, textBox);
        EndNarrationForTextBox(TextBox, textBox);
    }

    private void OpenRightTextBox(TextBox textBox)
    {
        OpenNarrationForTextBox(NarrationTextBox, textBox);
        OpenNarrationForTextBox(ConfirmationTextBox, textBox);
        OpenNarrationForTextBox(DecisionTextBox, textBox);
        OpenNarrationForTextBox(TextBox, textBox);
    }

    private void EndNarrationForTextBox(TextBox textBox, TextBox referenceTextBox)
    {
        if (textBox != null && textBox.gameObject.activeSelf && !textBox.Equals(referenceTextBox))
        {
            textBox.EndNarration();
            StartCoroutine(DeactivateTextBox(textBox));
        }
    }

    private IEnumerator DeactivateTextBox(TextBox textBox)
    {
        if (textBox != null && !textBox.DestroyTextBox)
        {
            while (!textBox.ClosedTextBox)
                yield return null;
            try
            {
                textBox.IsClosed = true;
                if (textBox.gameObject != null)
                    textBox.gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.LogWarning("WARNING: " + e.Message);
                EndDialogue();
            }
        }
    }

    private void OpenNarrationForTextBox(TextBox textBox, TextBox referenceTextBox)
    {
        if (textBox != null && textBox.Equals(referenceTextBox) && textBox.IsClosed)
        {
            textBox.gameObject.SetActive(true);
            if (textBox.gameObject.activeSelf)
                textBox.OpenTextBox();
            else
                StartCoroutine(ActivateTextBox(textBox));
        }
    }

    private IEnumerator ActivateTextBox(TextBox textBox)
    {
        if (textBox != null)
        {
            while (!textBox.gameObject.activeSelf)
            {
                textBox.gameObject.SetActive(true);
                yield return null;
            }
            textBox.OpenTextBox();
        }
    }

    public IEnumerator WaitUntilDialogueIsOver()
    {
        while (!DialogueStarted)
            yield return null;
        while (!DialogueEnded)
            yield return null;
    }

    private void SetUpDialogueVariables()
    {
        SetVariableState("playerName", Player.Instance().Name, "string");
        SetVariableState("itemName", _itemName, "string");
        SetVariableState("pluralName", _pluralName, "string");
        SetVariableState("itemType", _itemType, "string");

        if (CheckVariableState("numberOfWater", 0))
            SetVariableState("numberOfWater", _number, "int");

        if (CurrentStory.variablesState["pronouns"] != null)
            SetUpPronouns();
    }

    private void SetUpPronouns()
    {
        string sex = Player.Instance().Sex;
        switch (sex)
        {
            case "MALE":
                SetVariableState("subject", "he", "string");
                SetVariableState("subject_s", "he's", "string");
                SetVariableState("object", "him", "string");
                SetVariableState("possessive_a", "his", "string");
                SetVariableState("possessive_p", "his", "string");
                SetVariableState("reflexive", "himself", "string");
                SetVariableState("person", "man", "string");
                break;
            case "FEMALE":
                SetVariableState("subject", "she", "string");
                SetVariableState("subject_s", "she's", "string");
                SetVariableState("object", "her", "string");
                SetVariableState("possessive_a", "her", "string");
                SetVariableState("possessive_p", "hers", "string");
                SetVariableState("reflexive", "herself", "string");
                SetVariableState("person", "woman", "string");
                break;
            case "MALEFE":
                SetVariableState("subject", "they", "string");
                SetVariableState("subject_s", "they're", "string");
                SetVariableState("object", "them", "string");
                SetVariableState("possessive_a", "their", "string");
                SetVariableState("possessive_p", "theirs", "string");
                SetVariableState("reflexive", "themselves", "string");
                SetVariableState("person", "person", "string");
                break;
        }
    }

    public string SetVariableState(string variable, object value, string type)
    {
        string WARNING_MESSAGE_1 = "WARNING: The value for the ink variable " + variable + " is null.";
        string WARNING_MESSAGE_2 = "WARNING: The name of the variable you are trying to add is null.";
        string WARNING_MESSAGE_3 = "WARNING: The ink variable you are trying to add value to does not exist.";
        string WARNING_MESSAGE_4 = "WARNING: There is no CurrentStory present to add value to a VariableState.";
        string SUCCESS_MESSAGE = "SUCCESS";

        if (value == null)
        {
            //Debug.LogWarning($"WARNING: The value for the ink variable {variable} is null.");
            //return false;
            return WARNING_MESSAGE_1;
        }
        if (string.IsNullOrEmpty(variable))
        {
            //Debug.LogWarning($"WARNING: The name of the variable you are trying to add is null.");
            //return false;
            return WARNING_MESSAGE_2;
        }
        if (CurrentStory == null)
        {
            return WARNING_MESSAGE_4;
        }
        if (CurrentStory.variablesState[variable] == null)
        {
            //Debug.LogWarning($"WARNING: The ink variable you are trying to add value to does not exist.");
            // return false;
            return WARNING_MESSAGE_3;
        }

        try
        {
            CurrentStory.variablesState[variable] = value;
            return SUCCESS_MESSAGE;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            if (e.Message.Contains("FormatException"))
            {
                switch (type)
                {
                    case "string":
                        CurrentStory.variablesState[variable] = value.ToString();
                        break;
                    case "int":
                        CurrentStory.variablesState[variable] = (int)value;
                        break;
                }
            }
        }
        return SUCCESS_MESSAGE;
    }

    public object GetVariableState(string variable)
    {
        if (string.IsNullOrEmpty(variable))
        {
            //Debug.LogWarning("The variable name is null. Cannot retrieve value.");
            return "null";
        }
        if (CurrentStory.variablesState[variable] == null)
        {
            //Debug.LogWarning("The value for this is null");
            return "null";
        }

        return CurrentStory.variablesState[variable];
    }

    public bool CheckVariableState(string variable, object value)
    {
        if (string.IsNullOrEmpty(variable))
        {
            //Debug.LogWarning("The variable name for the ink file is null");
            return false;
        }
        if (value == null)
        {
            //Debug.LogWarning($"The value for this ink variable {variable} is null.");
            return false;
        }
        if (CurrentStory.variablesState[variable] == null)
        {
            //Debug.LogWarning($"There is no variable {variable} in the ink file with the variable name");
            return false;
        }

        return CurrentStory.variablesState[variable].Equals(value);
    }

    private void HandleStoryError(string message, Ink.ErrorType type)
    {
        if (type == Ink.ErrorType.Error)
        {
            Debug.LogError($"Ink Error: {message}");
        }
        else if (type == Ink.ErrorType.Warning)
        {
            Debug.LogWarning($"Ink Warning: {message}");
        }
        else
            Debug.Log($"Something happened with Ink that was not considered an error or warning {message}");

        EndDialogue();
    }

}