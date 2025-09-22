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

    /// <summary>
    /// Display DialougeData via the DialogueManager.
    /// </summary>
    public virtual void StartDialogue()
    {
        // if(Transform != null && TextBoxPrefab != null)
        //     TextBoxPrefab.gameObject.transform.SetParent(Transform);
        
        // try {
        //     TextBoxPrefab.gameObject.SetActive(true);
        //     TextBoxPrefab.OpenTextBox();
        //     TextBoxPrefab.StartNarration(DialogueData);
        // } catch(Exception e){
        //     Debug.LogWarning("WARNING: " + e.Message);
        //     DialogueManager.Instance.DisplayNextDialogue(DialogueData);
        // }

        //TODO: test if code works before deleting commented code
        if(Transform != null && TextBoxPrefab != null)
        {
            TextBoxPrefab.gameObject.transform.SetParent(Transform);
            TextBoxPrefab.gameObject.SetActive(true);
            TextBoxPrefab.OpenTextBox();
            TextBoxPrefab.StartNarration(DialogueData);
        }
        else
            DialogueManager.Instance.DisplayNextDialogue(DialogueData);
    }
}
