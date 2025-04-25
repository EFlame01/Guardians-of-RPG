using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// InteractableObject is a class that is designed
/// to give the player the ability to interact
/// with specific gameObjects in their playthrough.
/// </summary>
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] protected Light2D _myLight2D;
    [SerializeField] protected InputActionReference Select;
    protected bool CanInteract;
    protected bool CheckForInteraction;
    protected bool IsThisObjectDetected;
    protected static bool ObjectDetected;

    void Start()
    {
        HideInputSymbol();
    }

    virtual public void Update()
    {
        HandleInput();
        CheckPlayerInteraction();
    }

    /// <summary>
    /// Interacts with object. Each object will
    /// have a different way the player can interact
    /// with it.
    /// </summary>
    public abstract void InteractWithObject();

    /// <summary>
    /// Lights up the Light2D component connected
    /// to gameObject, indicating object is now
    /// interactable.
    /// </summary>
    public virtual void DisplayInputSymbol()
    {
        if(_myLight2D != null)
            _myLight2D.intensity = 1f;
    }

    /// <summary>
    /// Turns off Light2D component connected
    /// to gameObject, indicating object is not
    /// interactable.
    /// </summary>
    public void HideInputSymbol()
    {
        ObjectDetected = false;
        if(_myLight2D != null)
            _myLight2D.intensity = 0f; 
    }

    /// <summary>
    /// Uses boolean logic to determine if
    /// code should call the method
    /// InteractWithObject()
    /// </summary>
    protected void HandleInput()
    {
        if(Select.action.ReadValue<float>() > 0f && CanInteract)
            InteractWithObject();
    }

    /// <summary>
    /// Uses boolean logic to reveal an
    /// object as interactable.
    /// </summary>
    /// <param name="isObjectInteractable"></param>
    protected void RevealObjectIsInteractable(bool isObjectInteractable)
    {
        if(isObjectInteractable)
        {
            CanInteract = true;
            ObjectDetected = true;
            IsThisObjectDetected = true;
            DisplayInputSymbol();
        }
        else
        {
            CanInteract = false;
            ObjectDetected = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }

    /// <summary>
    /// Determine which side of the gameObject
    /// the player collided with.
    /// </summary>
    /// <returns></returns>
    protected PlayerDirection GetCollisionSide()
    {
        float xDiff = Mathf.Abs(PlayerSpawn.PlayerPosition.x - transform.position.x);
        float yDiff = Mathf.Abs(PlayerSpawn.PlayerPosition.y - transform.position.y);

        if(yDiff > xDiff)
        {
            //either up or down
            if(PlayerSpawn.PlayerPosition.y > transform.position.y)
                return PlayerDirection.UP;
            else
                return PlayerDirection.DOWN;
        }
        if(xDiff > yDiff)
        {
            //either left or right
            if(PlayerSpawn.PlayerPosition.x > transform.position.x)
                return PlayerDirection.RIGHT;
            else
                return PlayerDirection.LEFT;
        }
        else
            return PlayerDirection.NONE;
    }

    /// <summary>
    /// Checks if the player is still interacting with
    /// one of the InteractableObjects in the game.
    /// If it is not, it will set the PlayerState to 
    /// NOT_MOVING.
    /// </summary>
    private void CheckPlayerInteraction()
    {
        if(!CheckForInteraction)
            return;
        if(!GameManager.Instance.PlayerState.Equals(PlayerState.INTERACTING_WITH_OBJECT))
            return;
        if(DialogueManager.Instance.DialogueEnded)
        {
            GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
            CheckForInteraction = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            //either up or down
            if(PlayerSpawn.PlayerPosition.y > transform.position.y)
                transform.position = new Vector3(transform.position.x, transform.position.y, -5);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        }
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            //either up or down
            if(PlayerSpawn.PlayerPosition.y > transform.position.y)
                transform.position = new Vector3(transform.position.x, transform.position.y, -5);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y, 5);
        }
    }

}
