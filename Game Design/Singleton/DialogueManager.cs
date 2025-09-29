using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

/// <summary>
/// Class that inherites from PersistentSingleton class.
/// DialogueManager is reponsible for using the DialogueData
/// to diplay the panels required to reveal the dialogue and
/// interact with the story.
/// </summary>
public class DialogueManager : PersistentSingleton<DialogueManager>
{
    //Serialized variables
    public TextBox TextBox; //{get; private set;}
    public TextBoxConfirmation ConfirmationTextBox;
    public TextBoxDecision DecisionTextBox;
    public TextBox NarrationTextBox;

    //public variables
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

        //assigns global _dialogueData variable to use for further methods
        _dialogueData = dialogueData;

        //if current story does not exist or cannot continue
        if (CurrentStory == null || !CurrentStory.canContinue)
        {
            //this means we have not started dialogue yet
            if (!_dialogueEnded)
            {
                CurrentStory = new Story(_dialogueData.InkJSON.text);
                SetUpDialogueVariables();
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

        _textBoxType = (int)CurrentStory.variablesState["textBoxType"];

        if (_originalText == null || _originalText.Length <= 0)
            EndDialogue();
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
                        TextBox.OpenTextBox();
                        yield return new WaitForSeconds(0.25f);
                    }
                    _currentTextBox = TextBox;
                    break;
                case Units.NARRATION:
                    if (NarrationTextBox.IsClosed)
                    {
                        CloseRightTextBox(NarrationTextBox);
                        yield return new WaitForSeconds(0.4f);
                        NarrationTextBox.OpenTextBox();
                        yield return new WaitForSeconds(0.25f);
                    }
                    _currentTextBox = NarrationTextBox;
                    break;
                case Units.CONFIRMATION:
                    if (ConfirmationTextBox.IsClosed)
                    {
                        CloseRightTextBox(ConfirmationTextBox);
                        yield return new WaitForSeconds(0.4f);
                        ConfirmationTextBox.OpenTextBox();
                        yield return new WaitForSeconds(0.25f);
                    }
                    SetUpConfirmation();
                    _currentTextBox = ConfirmationTextBox;
                    break;
                case Units.DECISION:
                    if (DecisionTextBox.IsClosed)
                    {
                        CloseRightTextBox(DecisionTextBox);
                        yield return new WaitForSeconds(0.4f);
                        DecisionTextBox.OpenTextBox();
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
        GameManager.Instance.EnableNarrationInputs = true;
    }

    private void EndDialogue()
    {
        CurrentStory?.ResetState();
        CurrentStory = null;
        _currentTextBox.EndNarration();
        _dialogueEnded = false;
        DialogueEnded = true;
        DialogueContinued = false;
        CloseRightTextBox(null);
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

    private void ClickedOption(int index)
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
        }
    }

    private void SetUpDecision()
    {
        List<Choice> choices = CurrentStory.currentChoices;
        _clickedAlready = false;
        if (choices.Count <= 0 || _originalText.Length <= 0)
        {
            EndDialogue();
            return;
        }

        //Destroy previous options
        foreach (Transform child in DecisionTextBox.ListLayout)
            Destroy(child.gameObject);

        foreach (Choice choice in choices)
        {
            Button choiceBtn = Instantiate(DecisionTextBox.DecisionOptionPrefab, DecisionTextBox.ListLayout);
            DecisionTextBox.UpdateOptionButton(choiceBtn, choice.text);
            choiceBtn.onClick.AddListener(() => MakeDecision(choice, choices));
        }
    }

    private void MakeDecision(Choice choice, List<Choice> currentChoices)
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
                if (CurrentStory.variablesState["endDialogue"] != null && (string)CurrentStory.variablesState["endDialogue"] == "Yes")
                    CurrentStory.Continue();
                DisplayNextDialogue(_dialogueData);
                break;
            }
        }
    }

    private void CloseRightTextBox(TextBox textBox)
    {
        if (!NarrationTextBox.Equals(textBox) && !NarrationTextBox.IsClosed)
            NarrationTextBox.EndNarration();
        if (!ConfirmationTextBox.Equals(textBox) && !ConfirmationTextBox.IsClosed)
            ConfirmationTextBox.EndNarration();
        if (!DecisionTextBox.Equals(textBox) && !DecisionTextBox.IsClosed)
            DecisionTextBox.EndNarration();
        if (TextBox != null && !TextBox.Equals(textBox) && !TextBox.IsClosed)
            TextBox.EndNarration();
    }

    public IEnumerator WaitUntilDialogueIsOver()
    {
        while (!DialogueEnded)
            yield return null;
    }

    private void SetUpDialogueVariables()
    {
        if (CurrentStory.variablesState["playerName"] != null)
            CurrentStory.variablesState["playerName"] = Player.Instance().Name;

        if (CurrentStory.variablesState["itemName"] != null)
            CurrentStory.variablesState["itemName"] = _itemName;

        if (CurrentStory.variablesState["pluralName"] != null)
            CurrentStory.variablesState["pluralName"] = _pluralName;

        if (CurrentStory.variablesState["itemType"] != null)
            CurrentStory.variablesState["itemType"] = _itemType;

        if (CurrentStory.variablesState["numberOfWater"] != null && (int)CurrentStory.variablesState["numberOfWater"] == 0)
            CurrentStory.variablesState["numberOfWater"] = _number;

        if (CurrentStory.variablesState["pronouns"] != null)
            SetUpPronouns();
    }

    private void SetUpPronouns()
    {
        string sex = Player.Instance().Sex;
        switch (sex)
        {
            case "MALE":
                CurrentStory.variablesState["subject"] = "he";
                CurrentStory.variablesState["subject_s"] = "he's";
                CurrentStory.variablesState["object"] = "him";
                CurrentStory.variablesState["possessive_a"] = "his";
                CurrentStory.variablesState["possessive_p"] = "his";
                CurrentStory.variablesState["reflexive"] = "himself";
                CurrentStory.variablesState["person"] = "man";
                break;
            case "FEMALE":
                CurrentStory.variablesState["subject"] = "she";
                CurrentStory.variablesState["subject_s"] = "she's";
                CurrentStory.variablesState["object"] = "her";
                CurrentStory.variablesState["possessive_a"] = "her";
                CurrentStory.variablesState["possessive_p"] = "hers";
                CurrentStory.variablesState["reflexive"] = "herself";
                CurrentStory.variablesState["person"] = "woman";
                break;
            case "MALEFE":
                CurrentStory.variablesState["subject"] = "they";
                CurrentStory.variablesState["subject_s"] = "they're";
                CurrentStory.variablesState["object"] = "them";
                CurrentStory.variablesState["possessive_a"] = "their";
                CurrentStory.variablesState["possessive_p"] = "theirs";
                CurrentStory.variablesState["reflexive"] = "themselves";
                CurrentStory.variablesState["person"] = "person";
                break;
        }
    }

}