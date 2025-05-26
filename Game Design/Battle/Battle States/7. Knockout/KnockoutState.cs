using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/// <summary>
/// KnockoutState is a class that extends the 
/// <c>BattleState</c> class. This class is 
/// responsible for detailing which <c>Character</c>s
/// in the round have recently been knocked out.
/// 
/// Once that has been completed, it will determine
/// if the next state should be the <c>BattleOverState</c>
/// or the <c>CharacterActionState</c>.
/// </summary>
public class KnockoutState : BattleState
{
    //private variables
    private DialogueData dialogueData;
    private TextBox textBox;
    private string text;
    private bool startedDialogue;

    //Constructor
    public KnockoutState(DialogueData dialogueData, TextBox textBox)
    {
        this.dialogueData = dialogueData;
        this.textBox = textBox;
    }

    public override void Enter()
    {
        GetText();
        StartDialogue();
    }

    public override void Update()
    {
        if(startedDialogue && DialogueManager.Instance.DialogueEnded)
        {
            if(BattleOver())
                NextState = "BATTLE OVER STATE";
            else
            {
                //May need to check if prev state was character action or after round
                NextState = "CHARACTER ACTION STATE";
            }
        }
    }

    public override void Exit()
    {
        BattleSimStatus.RoundKnockOuts.Clear();
    }

    private void GetText()
    {
        text = "";

        for(int i = 0; i < BattleSimStatus.RoundKnockOuts.Count; i++)
        {
            if(i == 0)
                text += BattleSimStatus.RoundKnockOuts[i].Name + " ";
            else if (i + 1 == BattleSimStatus.RoundKnockOuts.Count)
                text += ", and " + BattleSimStatus.RoundKnockOuts[i].Name;
            else
                text += ", " + BattleSimStatus.RoundKnockOuts[i].Name + " ";
        }

        if(BattleSimStatus.RoundKnockOuts.Count > 1)
            text += " are knocked out!";
        if(BattleSimStatus.RoundKnockOuts.Count == 1)
            text += "is knocked out!";
    }

    private void StartDialogue()
    {
        DialogueManager.Instance.CurrentStory = new Story(dialogueData.InkJSON.text);
        DialogueManager.Instance.CurrentStory.variablesState["text"] = text;
        textBox.OpenTextBox();
        textBox.StartNarration(dialogueData);
        startedDialogue = true;
    }
}
