using UnityEngine;

public class MenuState : MonoBehaviour
{
    [SerializeField] private string menuName;
    [SerializeField] private Animator _subMenuAnimator;

    public virtual void Start()
    {
        AudioManager.Instance.PlaySoundEffect("open_01");
    }
    
    public void OnBackButton()
    {
        AudioManager.Instance.PlaySoundEffect("close_04");
        _subMenuAnimator.Play(menuName + "_close");
        MenuStateManager.MenuState = 0;
        Destroy(gameObject, 0.5f);
    }

    public void OnPlayerSettingsBackButtonPressed()
    {
        AudioManager.Instance.PlaySoundEffect("close_04");
        _subMenuAnimator.Play(menuName + "_close");
        MenuStateManager.MenuState = 0;
        GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        Destroy(gameObject, 0.5f);
    }
}
