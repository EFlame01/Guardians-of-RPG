using UnityEngine;
using System.Collections;
using System;

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
    private bool _error;

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
        try
        {
            DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
        }
        catch (Exception e)
        {
            Debug.LogError("ERROR: " + e.Message);
            GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        }
    }

    /// <summary>
    /// Opens the dialgoue box to reveal
    /// contents of the gameObject.
    /// </summary>
    /// <returns></returns>
    private void ReadObject()
    {
        try
        {
            StartDialogue();
            CheckForInteraction = true;
        }
        catch (Exception e)
        {
            Debug.Log("Could not read object: " + e.Message);
            GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
            _error = true;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
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
            if (!_error)
                _readSign = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}