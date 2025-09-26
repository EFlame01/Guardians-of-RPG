using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CutSceneObject : InteractableObject
{
    public CutScene CutScene;
    private bool _startedCutScene = false;

    /// <summary>
    /// If can interact, it activates
    /// the CutScene variable.
    /// </summary>
    public override void InteractWithObject()
    {
        if (CanInteract && !_startedCutScene)
        {
            _startedCutScene = true;
            CutScene.StartCutScene();
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collider2D)
    {
        if (ObjectDetected)
            return;

        if (collider2D.gameObject.CompareTag("Player"))
            RevealObjectIsInteractable(true);
    }

    public virtual void OnCollisionStay2D(Collision2D collider2D)
    {
        if (!ObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if (IsThisObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    public virtual void OnCollisionExit2D(Collision2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            _startedCutScene = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}