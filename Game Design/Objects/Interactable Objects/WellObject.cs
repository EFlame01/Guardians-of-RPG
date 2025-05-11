using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro.Examples;
using UnityEngine;

public class WellObject : InteractableObject, IDialogue
{
    [SerializeField] public string WellID;
    [SerializeField] public int NumberOfWater;
    [SerializeField] public DialogueData GetWaterDialogueData;
    [SerializeField] public DialogueData DontGetWaterDialogueData;
    [SerializeField] private GameObject _textBoxObject;
    private bool usingWell;
    
    private Item _water;
    private DialogueData _dialogueData;
    private Story _story;
    private WellData _wellData;
    private bool takesWater;


    public void OnEnable()
    {
        // _wellData = WellDataContainer.GetWellData(WellID);
        //TEST - Delete later 
        _wellData = new WellData(WellID, 1, NumberOfWater);
        WellDataContainer.WellDataList.Add(_wellData);
        //end of TEST
        NumberOfWater = _wellData == null ? NumberOfWater : _wellData.NumberOfWater;
    }

    public void Start()
    {

    }

    public override void InteractWithObject()
    {
        GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
        if(CanInteract && _wellData.DaysWithoutWater > 0 && !usingWell)
        {
            usingWell = true;
            GetWater();
        }
        else if (CanInteract && _wellData.DaysWithoutWater <= 0 && !usingWell)
        {
            usingWell = true;
            DontGetWater();
        }
        CheckForInteraction = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        _story = DialogueManager.Instance.CurrentStory;

        if(_story != null)
            UpdateStory();
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
        if(_story.variablesState["takesWater"] != null && (bool)_story.variablesState["takesWater"])
        {
            if(_wellData.DaysWithoutWater > 0)
                Player.Instance().Inventory.AddItem(_water.Name, _wellData.NumberOfWater);
            _wellData.DaysWithoutWater = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {
        if(ObjectDetected)
            return;

        if(collider2D.gameObject.tag.Equals("Player"))
            RevealObjectIsInteractable(true);
    }

    private void OnCollisionStay2D(Collision2D collider2D)
    {
        if(!ObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if(IsThisObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            usingWell = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}
