using UnityEngine;

public class MenuState : MonoBehaviour
{
    [SerializeField] private string menuName;
    [SerializeField] private Animator _subMenuAnimator;

    public virtual void Start()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.OPEN_UI_1);
    }
    
    public void OnBackButton()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.CLOSE_UI_4);
        _subMenuAnimator.Play(menuName + "_close");
        MenuStateManager.MenuState = 0;
        Destroy(gameObject, 0.5f);
    }

    public void OnPlayerSettingsBackButtonPressed()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.CLOSE_UI_4);
        _subMenuAnimator.Play(menuName + "_close");
        MenuStateManager.MenuState = 0;
        GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        Destroy(gameObject, 0.5f);
    }
}
