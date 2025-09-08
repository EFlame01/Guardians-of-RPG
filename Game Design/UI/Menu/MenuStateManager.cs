using UnityEngine;

public class MenuStateManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerIDMenuObject;
    [SerializeField] private GameObject _inventoryMenuObject;
    [SerializeField] private GameObject _moveSetMenuObject;
    [SerializeField] private GameObject _questMenuObject;
    [SerializeField] private GameObject _optionsMenuObject;
    [SerializeField] private Animator _mainMenuAnimator;

    public static int MenuState;
    private static bool waitForMainMenu;

    public void Start()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.OPEN_UI_1);
        MenuState = 0;
        waitForMainMenu = false;
        GameManager.Instance.PlayerState = PlayerState.PAUSED;
    }

    public void Update()
    {
        if(waitForMainMenu && MenuState == 0)
        {
            waitForMainMenu = false;
            _mainMenuAnimator.Play("reveal_settings_menu");
        }
    }

    public void SetMenuState(int nextState)
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.CLICK_1);
        MenuState = nextState;
        switch(nextState)
        {
            case 1:
                OpenMenuOption(_playerIDMenuObject);
                break;
            case 2:
                OpenMenuOption(_inventoryMenuObject);
                break;
            case 3:
                OpenMenuOption(_moveSetMenuObject);
                break;
            case 4:
                OpenMenuOption(_questMenuObject);
                break;
            case 5:
                OpenMenuOption(_optionsMenuObject);
                break;
            default:
                break;
        }
    }

    private void OpenMenuOption(GameObject menuObject)
    {
        waitForMainMenu = true;
        _mainMenuAnimator.Play("hide_settings_menu");
        Instantiate(menuObject);
    }

    public void OnExitMenu()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.CLOSE_UI_4);
        _mainMenuAnimator.Play("close_settings_menu");
        GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        Destroy(gameObject, 0.5f);
    }
}
