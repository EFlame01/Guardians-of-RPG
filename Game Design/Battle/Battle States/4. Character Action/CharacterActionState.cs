using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/// <summary>
/// CharacterActionState is a class that extends the 
/// <c>BattleState</c> class. This class takes all the
/// characters in battle and determines the order they 
/// should go in first. After this, it will announce
/// the chosen <c>Character</c>'s action.
/// 
/// Once the action is completed, it will determine
/// if the next state should be the <c>BattleOverState</c>
/// or the <c>OptionState</c>.
/// </summary>
public class CharacterActionState : BattleState, IDialogue
{
    //public variables
    public BattleOrderBST battleOrderBST;

    //private variables
    private TextBox _narrationTextBox;
    private DialogueData _dialogueData;
    // private bool _roundOver;
    private string _characterActionText;
    private bool _startedDialogue;

    //Constructor
    public CharacterActionState(DialogueData dialogueData, TextBox textBox)
    {
        CurrentState = Units.CHARACTER_ACTION_STATE;
        _dialogueData = dialogueData;
        _narrationTextBox = textBox;
    }

    public override void Enter()
    {
        //Determine order of operations if the round just started
        if(!BattleSimStatus.RoundStarted)
            DetermineOrder();
        //If the round is not over, show the character's action
        //Otherwise, determine which state to go to
        if(!RoundOver())
            DisplayAction();
        else
        {
            //TODO: test why RoundStarted is commented out
            // BattleSimStatus.RoundStarted = false;
            if(BattleSimStatus.RunSuccessful)
                NextState = Units.END_BATTLE;
            else if(BattleOver())
                NextState = Units.BATTLE_OVER_STATE;
            else
                NextState = Units.AFTER_ROUND_STATE;
                // NextState = "AFTER ROUND STATE";
        }
    }

    public override void Update()
    {
        if(_roundOver)
            return;
        if(_startedDialogue && DialogueManager.Instance.DialogueEnded)
        {
            if(HasAction(BattleSimStatus.ChosenCharacter))
                NextState = Units.ACTION_EFFECT_STATE;
            else
                NextState = Units.CHARACTER_ACTION_STATE;
        }
    }

    public override void Exit()
    {
        
    }

    private void DetermineOrder()
    {
        BattleSimStatus.RoundStarted = true;
        BattleOrderBST battleOrderBST = new BattleOrderBST();
        battleOrderBST.ArrangeBST();
    }

    private void DisplayAction()
    {
        Character character = BattleSimStatus.BattleQueue.Dequeue();
        BattleSimStatus.ChosenCharacter = character;

        if(character.BaseStats.Hp <= 0 && !character.BattleStatus.TurnStatus.Equals(TurnStatus.ITEM))
        {
            character.BattleStatus.SetTurnStatus(TurnStatus.CANNOT_MOVE);
            character.BattleStatus.SetTurnStatusTag(character.Name + " is knocked out and cannot move!");
        }

        _characterActionText = character.BattleStatus.TurnStatus switch
        {
            TurnStatus.ITEM => character.Name + " uses the " + character.BattleStatus.ChosenItem.Name + "!",
            TurnStatus.RUN => character.Name + " rolled to run!",
            TurnStatus.MOVE_FIRST => character.Name + " was able to move first!",
            TurnStatus.INITIATIVE => character.Name + " rolled for initiative!",
            TurnStatus.PROTECT => character.Name + " uses " + character.BattleStatus.ChosenMove.Name + "!",
            TurnStatus.COUNTER => character.Name + " uses " + character.BattleStatus.ChosenMove.Name + "!",
            TurnStatus.PRIORITY_1 => character.Name + " uses " + character.BattleStatus.ChosenMove.Name + "!",
            TurnStatus.PRIORITY_2 => character.Name + " uses " + character.BattleStatus.ChosenMove.Name + "!",
            TurnStatus.PRIORITY_3 => character.Name + " uses " + character.BattleStatus.ChosenMove.Name + "!",
            TurnStatus.FIGHT => character.Name + " uses " + character.BattleStatus.ChosenMove.Name + "!",
            TurnStatus.CANNOT_MOVE => character.BattleStatus.TurnStatusTag,
            _ => character.Name + " skips their turn..."
        };

        StartDialogue();
    }

    // private bool RoundOver()
    // {
    //     _roundOver = BattleSimStatus.BattleQueue.Count == 0;
    //     return _roundOver;
    // }

    public void StartDialogue()
    {
        _startedDialogue = true;
        DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);
        DialogueManager.Instance.CurrentStory.variablesState["text"] = _characterActionText;
        _narrationTextBox.OpenTextBox();
        _narrationTextBox.StartNarration(_dialogueData);
    }

    private bool HasAction(Character character)
    {
        switch(character.BattleStatus.TurnStatus)
        {
            case TurnStatus.SKIP:
            case TurnStatus.CANNOT_MOVE:
            case TurnStatus.NOTHING:
                return false;
            default:
                return true;
        }
    }
}