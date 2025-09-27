using UnityEngine;
using System.Collections;

/// <summary>
/// SignObject is a class that extends the
/// InteractableObject. SignObjects are objects
/// that the player can read.
/// </summary>
public class SignObject : InteractableObject, IDialogue
{
    //Serialized variables
    [SerializeField] private PlayerDirection _directionToReadSign;
    [SerializeField] private GameObject _textBoxObject;
    [SerializeField] private DialogueData _dialogueData;

    //private variable
    private bool _readSign;

    /// <summary>
    /// If can interact, calls the method
    /// ReadObject().
    /// </summary>
    public override void InteractWithObject()
    {
        if (CanInteract && !_readSign)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _readSign = true;
            ReadObject();
        }
    }

    public void StartDialogue()
    {
        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
    }

    /// <summary>
    /// Opens the dialgoue box to reveal
    /// contents of the gameObject.
    /// </summary>
    /// <returns></returns>
    private void ReadObject()
    {
        StartDialogue();
        CheckForInteraction = true;
    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {
        if (ObjectDetected)
            return;

        if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToReadSign) && GetObjectFacingSide().Equals(_directionToReadSign))
            RevealObjectIsInteractable(true);
    }

    private void OnCollisionStay2D(Collision2D collider2D)
    {
        if (!ObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToReadSign) && GetObjectFacingSide().Equals(_directionToReadSign))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if (IsThisObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToReadSign) && GetObjectFacingSide().Equals(_directionToReadSign))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            _readSign = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}