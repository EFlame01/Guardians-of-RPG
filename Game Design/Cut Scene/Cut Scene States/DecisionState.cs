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
    public TextBoxDecision TextBoxDecision;
    public DialogueData DialogueData;
    public CutSceneState[] StateOptions;

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

    /// <summary>
    /// If the state is active and an option is selected,
    /// then this method checks what the decision is,
    /// determines the next state based on the decision,
    /// and resets the decision state in the DialogueManager.
    /// </summary>
    private void CheckDecision()
    {
        if (IsActive && OptionSelected())
        {
            NextState = StateOptions[DialogueManager.Instance.GetDecision()];
            DialogueManager.Instance.ResetDecision();
            Exit();
        }
    }

    /// <summary>
    /// Checks if an option was selected in
    /// the DialogueManager. If an option was selected,
    /// it will return TRUE. Otherwise, it will
    /// return false.
    /// </summary>
    /// <returns>TRUE if decision is greater than -1; FALSE otherwise </return>
    private bool OptionSelected()
    {
        return DialogueManager.Instance.GetDecision() > -1;
    }

    /// <summary>
    /// Displays the decision options.
    /// </summary>
    public void StartDialogue()
    {
        if (TextBoxDecision != null)
            TextBoxDecision.StartNarration(DialogueData);
        else
            DialogueManager.Instance.DisplayNextDialogue(DialogueData);
    }
}
