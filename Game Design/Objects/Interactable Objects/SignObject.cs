using UnityEngine;
using System.Collections;

/// <summary>
/// SignObject is a class that extends the
/// InteractableObject. SignObjects are objects
/// that the player can read.
/// </summary>
public class SignObject : InteractableObject, IDialogue
{
    [SerializeField] private PlayerDirection _directionToReadSign;
    [SerializeField] private GameObject _textBoxObject;
    [SerializeField] private DialogueData _dialogueData;

    private bool _readSign;

    /// <summary>
    /// If can interact, calls the method
    /// ReadObject().
    /// </summary>
    public override void InteractWithObject()
    {
        if(CanInteract && !_readSign)
        {
            _readSign = true;
            ReadObject();
        }
    }

    public void StartDialogue()
    {
        TextBox textBox = Instantiate(_textBoxObject).GetComponent<TextBox>();
        textBox.OpenTextBox();
        textBox.StartNarration(_dialogueData);
    }

    /// <summary>
    /// Opens the dialgoue box to reveal
    /// contents of the gameObject.
    /// </summary>
    /// <returns></returns>
    private void ReadObject()
    {
        StartDialogue();
    }

    /// <summary>
    /// Determines which side the player
    /// should be facing to read the sign.
    /// </summary>
    /// <returns>side to read sign.</returns>
    private PlayerDirection GetSignFacingSide()
    {
        return GetCollisionSide() switch
        {
            PlayerDirection.UP => PlayerDirection.DOWN,
            PlayerDirection.DOWN => PlayerDirection.UP,
            PlayerDirection.LEFT => PlayerDirection.RIGHT,
            PlayerDirection.RIGHT => PlayerDirection.LEFT,
            _ => PlayerDirection.DOWN,
        };
    }

    private void OnCollisionEnter2D(Collision2D collider2D)
    {
        if(ObjectDetected)
            return;

        if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToReadSign) && GetSignFacingSide().Equals(_directionToReadSign))
            RevealObjectIsInteractable(true);
    }

    private void OnCollisionStay2D(Collision2D collider2D)
    {
        if(!ObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToReadSign) && GetSignFacingSide().Equals(_directionToReadSign))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if(IsThisObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToReadSign) && GetSignFacingSide().Equals(_directionToReadSign))
                RevealObjectIsInteractable(true);  
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            _readSign = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}