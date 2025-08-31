using System.Collections;
using UnityEngine;

public class FightObjectEyeLine : MonoBehaviour
{
    [SerializeField] public PlayerDirection direction;
    [SerializeField] public FightObject fightObject;
    private bool ConfrontPlayer = true;

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Fight") && InEyeLine() && ConfrontPlayer && IsInteractable())
        {
            ConfrontPlayer = false;
            fightObject.PlayerViewDirection = direction;
            StartCoroutine(fightObject.ConfrontPlayer2());
        }
    }

    public void OnTriggerStay2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Fight") && InEyeLine() && ConfrontPlayer && IsInteractable())
        {
            ConfrontPlayer = false;
            fightObject.PlayerViewDirection = direction;
            StartCoroutine(fightObject.ConfrontPlayer2());
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Fight") && InEyeLine() && ConfrontPlayer && IsInteractable())
        {
            ConfrontPlayer = true;
            fightObject.PlayerViewDirection = PlayerDirection.NONE;
        }    
    }

    private bool InEyeLine()
    {
        return fightObject.NPCDirection == direction;
    }

    private bool IsInteractable()
    {
        return GameManager.Instance.PlayerState.Equals(PlayerState.NOT_MOVING) || GameManager.Instance.PlayerState.Equals(PlayerState.MOVING);
    }
}