using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DecisionState is a class that extends from the 
/// <c>CutSceneState</c> class. This class opens
/// the decision box for the player to choose which
/// option to go through during the cut scene.
/// </summary>
public class DecisionState : CutSceneState, IDialogue
{
    //serialized variables
    [SerializeField] public TextBoxDecision TextBoxDecision;
    [SerializeField] public DialogueData DialogueData;
    [SerializeField] public CutSceneState[] StateOptions;

    public override void Enter()
    {
        base.Enter();
        DialogueManager.Instance.ResetDecision();
        StartDialogue();
    }

    public override void Update()
    {
        CheckDecision();
    }

    private void CheckDecision()
    {
        if(IsActive && OptionSelected())
        {
            NextState = StateOptions[DialogueManager.Instance.GetDecision()];
            DialogueManager.Instance.ResetDecision();
            Exit();
        }
    }

    private bool OptionSelected()
    {
        return DialogueManager.Instance.GetDecision() > -1;
    }

    public void StartDialogue()
    {
        if(TextBoxDecision != null)
            TextBoxDecision.StartNarration(DialogueData);
        else
            DialogueManager.Instance.DisplayNextDialogue(DialogueData);
    }
}
