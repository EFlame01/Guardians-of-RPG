using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro.Examples;
using UnityEngine;

/// <summary>
/// WellObject is a class that extends the
/// InteractableObject. WellObjects are objects
/// that the player can interact with
/// to recieve water a limited amount of times
/// per day.
/// </summary>
public class WellObject : InteractableObject, IDialogue
{
    //Serialized variables
    [SerializeField] private string WellID;
    [SerializeField] private int NumberOfWater;
    [SerializeField] private DialogueData GetWaterDialogueData;
    [SerializeField] private DialogueData DontGetWaterDialogueData;

    //private variables
    private bool _usingWell;
    private Item _water;
    private DialogueData _dialogueData;
    private Story _story;
    private WellData _wellData;

    public void OnEnable()
    {
        _wellData = WellDataContainer.GetWellData(WellID);
        _wellData ??= new WellData(WellID, 1, NumberOfWater);
        NumberOfWater = _wellData == null ? NumberOfWater : _wellData.NumberOfWater;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        _story = DialogueManager.Instance.CurrentStory;

        if (_story != null)
            UpdateStory();
    }

    public override void InteractWithObject()
    {
        GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
        if (CanInteract && _wellData.DaysWithoutWater > 0 && !_usingWell)
        {
            _usingWell = true;
            GetWater();
        }
        else if (CanInteract && _wellData.DaysWithoutWater <= 0 && !_usingWell)
        {
            _usingWell = true;
            DontGetWater();
        }
        CheckForInteraction = true;
    }

    public void StartDialogue()
    {
        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
    }

    private void GetWater()
    {
        _dialogueData = GetWaterDialogueData;
        _water = ItemMaker.Instance.GetItemBasedOnName("Water");
        DialogueManager.Instance.SetItemName(_water.Name);
        DialogueManager.Instance.SetPluralName(_water.PluralName);
        DialogueManager.Instance.SetItemType("FOOD");
        DialogueManager.Instance.SetNumber(NumberOfWater);
        StartDialogue();
    }

    private void DontGetWater()
    {
        _dialogueData = DontGetWaterDialogueData;
        StartDialogue();
    }

    public void UpdateStory()
    {
        if (_story.variablesState["takesWater"] != null && (bool)_story.variablesState["takesWater"])
        {
            if (_wellData.DaysWithoutWater > 0)
                Player.Instance().Inventory.AddItem(_water.Name, _wellData.NumberOfWater);
            _wellData.DaysWithoutWater = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {
        if (ObjectDetected)
            return;

        if (collider2D.gameObject.CompareTag("Player"))
            RevealObjectIsInteractable(true);
    }

    private void OnCollisionStay2D(Collision2D collider2D)
    {
        if (!ObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if (IsThisObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            _usingWell = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}
