using UnityEngine;
using System;

/// <summary>
/// DialogueState is a class that extends from the 
/// <c>CutSceneState</c> class. This class opens
/// the dialogue box to be read.
/// </summary>
public class DialogueState : CutSceneState, IDialogue
{
    //serialized variables
    [SerializeField] protected TextBox TextBoxPrefab;
    [SerializeField] protected Transform Transform;
    [SerializeField] protected DialogueData DialogueData;

    public override void Enter()
    {
        base.Enter();
        StartDialogue();
    }

    public override void Update()
    {
        CheckDialogue();
    }

    public void CheckDialogue()
    {
        if(IsActive && DialogueManager.Instance.DialogueEnded)
            Exit();
    }

    public virtual void StartDialogue()
    {
        if(Transform != null)
            TextBoxPrefab.gameObject.transform.SetParent(Transform);
        
        try {
            TextBoxPrefab.gameObject.SetActive(true);
            TextBoxPrefab.OpenTextBox();
            TextBoxPrefab.StartNarration(DialogueData);
        } catch(Exception e){
            Debug.LogWarning("TextBoxPrefab is missing" + e.ToString());
            DialogueManager.Instance.DisplayNextDialogue(DialogueData);
        }
    }
}
