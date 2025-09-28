using UnityEngine;

/// <summary>
/// PlayerSprite is a class that displays
/// the sprite of a character and their
/// movements in game.
/// </summary>
public class PlayerSprite : MonoBehaviour
{
    [SerializeField] private string CharacterName;
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Sprite maleSprite;
    [SerializeField] private Sprite femaleSprite;
    [SerializeField] private Animator Animator;

    public void Start()
    {
        if (CharacterName.Equals("player"))
            CharacterName = Player.Instance().MaleOrFemale().Equals("MALE") ? "adam" : "eve";
        if (gameObject.CompareTag("Player"))
        {
            PerformIdleAnimation(PlayerDirection.UP);
            image.sprite = Player.Instance().MaleOrFemale().Equals("MALE") ? maleSprite : femaleSprite;
        }
    }

    /// <summary>
    /// Plays the animation for the sprite
    /// to walk based on the walkDirection.
    /// If walkDirection is "none" or if 
    /// the runAnimatorController == null,
    /// it will not do anything.
    /// </summary>
    /// <param name="walkDirection"></param>
    public void PerformWalkAnimation(string walkDirection)
    {
        if (Animator.runtimeAnimatorController == null)
            return;

        if (walkDirection == null || walkDirection.Equals("none"))
            return;

        Animator.Play(CharacterName + "_" + walkDirection);
    }

    /// <summary>
    /// Plays the idle animation based on the 
    /// playerDirection. If runAnimatorController == null,
    /// it will not do anything.
    /// </summary>
    /// <param name="playerDirection"></param>
    public void PerformIdleAnimation(PlayerDirection playerDirection)
    {
        if (Animator.runtimeAnimatorController == null)
            return;

        switch (playerDirection)
        {
            case PlayerDirection.UP:
                Animator.Play(CharacterName + "_idle_up");
                break;
            case PlayerDirection.DOWN:
                Animator.Play(CharacterName + "_idle_down");
                break;
            case PlayerDirection.LEFT:
                Animator.Play(CharacterName + "_idle_left");
                break;
            case PlayerDirection.RIGHT:
                Animator.Play(CharacterName + "_idle_right");
                break;
            default:
                Animator.Play(CharacterName + "_idle_down");
                break;
        }
    }
}
