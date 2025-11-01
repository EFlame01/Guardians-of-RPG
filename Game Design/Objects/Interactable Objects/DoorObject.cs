using System.Collections;
using UnityEngine;

/// <summary>
/// DoorObject is a class that extends
/// the InteractableObject class. DoorObjects
/// allow the player to open a door to another
/// scene once those doors are interactable.
/// </summary>
public class DoorObject : InteractableObject
{
    [SerializeField] private ObjectSprite _doorSprite;
    [SerializeField] private string _nameOfSoundEffect;
    [SerializeField] private Portal _portal;

    /// <summary>
    /// If player can interact, calls the
    /// OpenDoor() method.
    /// </summary>
    public override void InteractWithObject()
    {
        if (CanInteract)
            StartCoroutine(OpenDoor());
    }

    /// <summary>
    /// Opens a door and sends player to a new
    /// scene.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OpenDoor()
    {
        GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
        AudioManager.Instance.PlaySoundEffect(_nameOfSoundEffect);
        _doorSprite.OpenAnimation();
        yield return new WaitForSeconds(1f);

        _portal.SendToNewLocation();
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (ObjectDetected)
            return;

        if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(PlayerDirection.UP))
            RevealObjectIsInteractable(true);
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (!ObjectDetected)
        {
            //see if object should be detected
            if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(PlayerDirection.UP))
                RevealObjectIsInteractable(true);
        }
        else if (IsThisObjectDetected)
        {
            //if an object is detected and it is this object that is detected
            //check to see if object should stay detected
            if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(PlayerDirection.UP))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
        else
            RevealObjectIsInteractable(false);
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player") && IsThisObjectDetected)
        {
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}