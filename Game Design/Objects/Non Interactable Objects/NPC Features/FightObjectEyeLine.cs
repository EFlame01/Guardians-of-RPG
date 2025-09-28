using UnityEngine;

/// <summary>
/// FightObjectEyeLine is a class that
/// allows the <c>FightObject</c> class
/// to see the player
/// </summary>
public class FightObjectEyeLine : MonoBehaviour
{
    //Serialized varialbes
    [SerializeField] private PlayerDirection direction;
    [SerializeField] private FightObject fightObject;

    //private variable
    private bool ConfrontPlayer = true;

    /// <summary>
    /// This function chekcs if the <c>FightObject</c>
    /// is facing the player.
    /// </summary>
    /// <returns><c>TRUE</c> if <c>FightObject</c> is facing the player. Otherwise, it returns <c>FALSE</c></returns>
    private bool InEyeLine()
    {
        return fightObject.NPCDirection == direction;
    }

    /// <summary>
    /// Checks if the PlayerState is in a state
    /// where the FightObject can initiate 
    /// the interaction.
    /// </summary>
    /// <returns>TRUE if the PlayerState is MOVING or NOT_MOVING. Otherwise, it returns FALSE</returns>
    private bool IsInteractable()
    {
        return GameManager.Instance.PlayerState.Equals(PlayerState.NOT_MOVING) || GameManager.Instance.PlayerState.Equals(PlayerState.MOVING);
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Fight") && InEyeLine() && ConfrontPlayer && IsInteractable())
        {
            ConfrontPlayer = false;
            fightObject.PlayerViewDirection = direction;
            StartCoroutine(fightObject.ConfrontPlayer2());
        }
    }

    public void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Fight") && InEyeLine() && ConfrontPlayer && IsInteractable())
        {
            ConfrontPlayer = false;
            fightObject.PlayerViewDirection = direction;
            StartCoroutine(fightObject.ConfrontPlayer2());
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Fight") && InEyeLine() && ConfrontPlayer && IsInteractable())
        {
            ConfrontPlayer = true;
            fightObject.PlayerViewDirection = PlayerDirection.NONE;
        }
    }
}