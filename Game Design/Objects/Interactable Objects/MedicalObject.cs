using System.Collections;
using UnityEngine;

public class MedicalObject : InteractableObject
{
    [SerializeField] private PlayerDirection _directionToUseMedicalCenter;

    private bool _useMedicalCenter;

    public override void InteractWithObject()
    {
        if(CanInteract && !_useMedicalCenter)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _useMedicalCenter = true;
            UseMedicalCenter();
        }
    }

    private void UseMedicalCenter()
    {
        Debug.Log("MEDICAL SHOP IN USE");
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(ObjectDetected)
            return;

        if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMedicalCenter) && GetObjectFacingSide().Equals(_directionToUseMedicalCenter))
            RevealObjectIsInteractable(true);
    }
    
    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if(!ObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMedicalCenter) && GetObjectFacingSide().Equals(_directionToUseMedicalCenter))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if(IsThisObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMedicalCenter) && GetObjectFacingSide().Equals(_directionToUseMedicalCenter))
                RevealObjectIsInteractable(true);  
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            _useMedicalCenter = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}